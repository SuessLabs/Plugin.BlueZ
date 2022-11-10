using System;
using System.Threading.Tasks;
using Tmds.DBus;

namespace Plugin.BlueZ
{
  public class Server : IDisposable
  {
    public Server()
    {
      Connection = new Connection(Address.System);
    }

    public Connection Connection { get; }

    public async Task ConnectAsync()
    {
      await Connection.ConnectAsync();
    }

    public void Dispose()
    {
      Connection.Dispose();
    }
  }
}
