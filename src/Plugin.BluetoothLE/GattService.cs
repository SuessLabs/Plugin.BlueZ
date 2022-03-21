using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.BluetoothLE
{
  public abstract class GattService : IGattService
  {
    protected GattService(IDevice device, Guid uuid)
    {
      Device = device;
      Id = uuid;
    }

    public Guid Id { get; }

    public string Name { get; }

    public IDevice Device { get; }

    public Task<IGattCharacteristic> GetCharacteristicAsync(Guid characteristicId)
    {
      throw new NotImplementedException();
    }

    public Task<IReadOnlyList<IGattCharacteristic>> GetCharacteristicsAsync()
    {
      throw new NotImplementedException();
    }
  }
}
