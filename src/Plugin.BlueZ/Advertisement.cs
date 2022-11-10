using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tmds.DBus;

namespace Plugin.BlueZ
{
  /// <summary>Advertisement</summary>
  /// <remarks>https://github.com/bluez/bluez-sandbox/blob/master/doc/advertising-api.txt</remarks>
  ////public class Advertisement : BaseProperties<AdvertisementProperties>, ILEAdvertisement1
  public class Advertisement : BaseProperties<LEAdvertisement1Properties>, ILEAdvertisement1
  {
    public Advertisement(string objectPath, LEAdvertisement1Properties properties)
      : base(objectPath, properties)
    {
    }

    public Task ReleaseAsync()
    {
      throw new NotImplementedException();
    }
  }

  public class BaseProperties<T>
  {
    protected readonly T Properties;

    protected BaseProperties(ObjectPath objectPath, T properties)
    {
      ObjectPath = objectPath;
      Properties = properties;
    }

    public ObjectPath ObjectPath { get; }

    public Task<object> GetAsync(string prop)
    {
      return Task.FromResult(Properties.ReadProperty(prop));
    }
    public Task<T> GetAsync<T>(string prop)
    {
      return Task.FromResult(Properties.ReadProperty<T>(prop));
    }

    public Task<T> GetAllAsync()
    {
      return Task.FromResult(Properties);
    }

    public Task SetAsync(string prop, object val)
    {
      return Properties.SetProperty(prop, val);
    }

    public Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler)
    {
      return SignalWatcher.AddAsync(this, nameof(OnPropertiesChanged), handler);
    }

    public event Action<PropertyChanges> OnPropertiesChanged;
  }

  public static class PropertyAccessExtensions
  {
    public static T ReadProperty<T>(this object o, string prop)
    {
      var propertyValue = o.GetType().GetProperty(prop)?.GetValue(o);
      return (T)propertyValue;
    }

    public static object ReadProperty(this object o, string prop)
    {
      return o.GetType().GetProperty(prop)?.GetValue(o);
    }


    public static Task SetProperty(this object o, string prop, object val)
    {
      o.GetType().GetProperty(prop)?.SetValue(o, val);
      return Task.CompletedTask;
    }
  }
}
