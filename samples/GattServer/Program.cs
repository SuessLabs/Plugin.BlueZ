// See https://aka.ms/new-console-template for more information
using Plugin.BlueZ;
using Tmds.DBus;

Console.WriteLine("Hello, World!");

using (var svr = new Server())
{
  await svr.ConnectAsync();
  await Advertisement.Register(svr);
}


public class Advertisement
{
  public void Register(Server server)
  {
    var properties = AdvertisementProperties
      {

    }
  }

}

