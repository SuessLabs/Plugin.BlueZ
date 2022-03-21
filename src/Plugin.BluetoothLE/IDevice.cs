using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Plugin.BluetoothLE
{
  public interface IDevice
  {
    /// <summary>UUID of the device.</summary>
    Guid Id { get; }  // TODO: Should this be a string, to play it safe?

    /// <summary>Advertised name of the device.</summary>
    string Name { get; }

    /// <summary>
    /// Requests a MTU update and fires an "Exchange MTU Request" on the ble stack.
    /// Be aware that the resulting MTU value will be negotiated between master and slave
    /// using your requested value for the negotiation.
    /// </summary>
    int MtuSize { get; }

    /// <summary>The RSSI of the connected device.</summary>
    int Rssi { get; }

    ConnectionState State { get; }

    /// <summary>Gets all services of the device.</summary>
    /// <param name="cancellationToken"></param>
    /// <returns>A task that represents the asynchronous read operation. The Result property will contain a list of all available services.</returns>
    Task<IReadOnlyList<IGattService>> GetServicesAsync(CancellationToken cancellationToken = default);

    /// <summary>Gets the RSSI of the connected device.</summary>
    /// <returns></returns>
    IObservable<int> UpdateRssiAsync();

  }
}
