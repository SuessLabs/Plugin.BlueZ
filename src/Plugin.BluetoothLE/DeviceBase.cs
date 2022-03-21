using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Plugin.BluetoothLE
{
  public abstract class DeviceBase : IDevice
  {
    public Guid Id { get; protected set; }

    public string Name { get; protected set; }

    public int MtuSize { get; } = 20;

    public int Rssi { get; }

    public ConnectionState State { get; }

    public Task<IReadOnlyList<IGattService>> GetServicesAsync(CancellationToken cancellationToken = default)
    {
      throw new NotImplementedException();
    }

    public IObservable<int> UpdateRssiAsync()
    {
      throw new NotImplementedException();
    }
  }
}
