using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Plugin.BlueZ.Extensions;
using Tmds.DBus;

namespace Plugin.BlueZ
{
  public delegate Task DeviceChangeEventHandlerAsync(Adapter sender, DeviceFoundEventArgs eventArgs);

  public delegate Task AdapterEventHandlerAsync(Adapter sender, BlueZEventArgs eventArgs);

  /// <summary>
  /// Add events to IAdapter1.
  /// </summary>
  public class Adapter : IAdapter1, IDisposable
  {
    private IAdapter1 _proxy;
    private IDisposable _interfacesWatcher;
    private IDisposable _propertyWatcher;
    private DeviceChangeEventHandlerAsync _deviceFound;
    private AdapterEventHandlerAsync _poweredOn;

    ~Adapter()
    {
      Dispose();
    }

    internal static async Task<Adapter> CreateAsync(IAdapter1 proxy)
    {
      var adapter = new Adapter
      {
        _proxy = proxy,
      };

      var objectManager = Connection.System.CreateProxy<IObjectManager>(BluezConstants.DbusService, "/");
      adapter._interfacesWatcher = await objectManager.WatchInterfacesAddedAsync(adapter.OnDeviceAddedAsync);
      adapter._propertyWatcher = await proxy.WatchPropertiesAsync(adapter.OnPropertyChanges);

      return adapter;
    }

    public void Dispose()
    {
      _interfacesWatcher?.Dispose();
      _interfacesWatcher = null;

      GC.SuppressFinalize(this);
    }

    public event DeviceChangeEventHandlerAsync DeviceFound
    {
      add
      {
        _deviceFound += value;
        FireEventForExistingDevicesAsync();
      }
      remove
      {
        _deviceFound -= value;
      }
    }

    public event AdapterEventHandlerAsync PoweredOn
    {
      add
      {
        _poweredOn += value;
        FireEventIfPropertyAlreadyTrueAsync(_poweredOn, "Powered");
      }
      remove
      {
        _poweredOn -= value;
      }
    }

    public event AdapterEventHandlerAsync PoweredOff;

    public ObjectPath ObjectPath => _proxy.ObjectPath;

    public Task<Adapter1Properties> GetAllAsync()
    {
      return _proxy.GetAllAsync();
    }

    /// <summary>Name of Adapter (i.e. "/org/bluez/hci0").</summary>
    public string Name => ObjectPath.ToString();

    public Task<T> GetAsync<T>(string prop)
    {
      return _proxy.GetAsync<T>(prop);
    }

    /// <summary>Return available filters that can be given to SetDiscoveryFilter.</summary>
    /// <returns>String of filters.</returns>
    public Task<string[]> GetDiscoveryFiltersAsync()
    {
      return _proxy.GetDiscoveryFiltersAsync();
    }

    public Task RemoveDeviceAsync(ObjectPath Device)
    {
      return _proxy.RemoveDeviceAsync(Device);
    }

    /// <summary>Set Property Value Async.</summary>
    /// <param name="prop"></param>
    /// <param name="val"></param>
    /// <returns></returns>
    public Task SetAsync(string prop, object val)
    {
      return _proxy.SetAsync(prop, val);
    }

    /// <summary>
    /// This method sets the device discovery filter for the
    /// caller. When this method is called with no filter
    /// parameter, filter is removed.
    /// </summary>
    /// <param name="properties">Filter parameters. Ref: <see cref="https://git.kernel.org/pub/scm/bluetooth/bluez.git/tree/doc/adapter-api.txt"/>.</param>
    /// <returns></returns>
    public Task SetDiscoveryFilterAsync(IDictionary<string, object> properties)
    {
      return _proxy.SetDiscoveryFilterAsync(properties);
    }

    /// <summary>Scan for devices nearby.</summary>
    /// <returns>Task.</returns>
    public Task StartDiscoveryAsync()
    {
      return _proxy.StartDiscoveryAsync();
    }

    /// <summary>Stop scanning for devices nearby.</summary>
    /// <returns>Task.</returns>
    public Task StopDiscoveryAsync()
    {
      return _proxy.StopDiscoveryAsync();
    }

    /// <summary>Watch for property updates.</summary>
    /// <param name="handler">Handler with argument of <seealso cref="PropertyChanges"/>.</param>
    /// <returns>Disposable task.</returns>
    public Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler)
    {
      return _proxy.WatchPropertiesAsync(handler);
    }

    private async void FireEventForExistingDevicesAsync()
    {
      var devices = await this.GetDevicesAsync();
      foreach (var device in devices)
      {
        _deviceFound?.Invoke(this, new DeviceFoundEventArgs(device, isStateChange: false));
      }
    }

    private async void OnDeviceAddedAsync((ObjectPath objectPath, IDictionary<string, IDictionary<string, object>> interfaces) args)
    {
      if (BlueZManager.IsMatch(BluezConstants.DeviceInterface, args.objectPath, args.interfaces, this))
      {
        var device = Connection.System.CreateProxy<IDevice1>(BluezConstants.DbusService, args.objectPath);

        var dev = await Device.CreateAsync(device);
        _deviceFound?.Invoke(this, new DeviceFoundEventArgs(dev));
      }
    }

    private async void FireEventIfPropertyAlreadyTrueAsync(AdapterEventHandlerAsync handler, string prop)
    {
      try
      {
        var value = await _proxy.GetAsync<bool>(prop);
        if (value)
        {
          // TODO: Suppress duplicate event from OnPropertyChanges.
          handler?.Invoke(this, new BlueZEventArgs(isStateChange: false));
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error checking if '{prop}' is already true: {ex}");
      }
    }

    private void OnPropertyChanges(PropertyChanges changes)
    {
      foreach (var pair in changes.Changed)
      {
        switch (pair.Key)
        {
          case "Powered":
            if (true.Equals(pair.Value))
            {
              _poweredOn?.Invoke(this, new BlueZEventArgs());
            }
            else
            {
              PoweredOff?.Invoke(this, new BlueZEventArgs());
            }

            break;
        }
      }
    }
  }
}
