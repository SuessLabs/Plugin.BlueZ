using System.Collections.Generic;
using Tmds.DBus;

namespace Plugin.BlueZ
{
  [Dictionary]
  public class AdvertisementProperties
  {
    public string Type { get; set; }

    public string[] ServiceUUIDs { get; set; }

    public IDictionary<string, object> ManufacturerData { get; set; }

    public string[] SolicitUUIDs { get; set; }

    public IDictionary<string, object> ServiceData { get; set; }

    public bool IncludeTxPower { get; set; }

    public string LocalName { get; set; }
  }
}
