using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.BluetoothLE
{
  public interface IGattService
  {
    Guid Id { get; }

    string Name { get; }

    IDevice Device { get; }

    Task<IReadOnlyList<IGattCharacteristic>> GetCharacteristicsAsync();

    /// <summary>Gets the first characteristic wit the Id <paramref name="characteristicId"/>.</summary>
    /// <param name="characteristicId">Characteristic Id to search for.</param>
    /// <returns></returns>
    Task<IGattCharacteristic> GetCharacteristicAsync(Guid characteristicId);
  }
}
