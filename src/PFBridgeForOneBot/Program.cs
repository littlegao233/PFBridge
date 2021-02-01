using System;
using System.Threading.Tasks;
using Sora.Server;
namespace PFBridgeForOneBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            SoraWSServer server = new SoraWSServer(new ServerConfig()
            {
                ApiPath = "api",
                EventPath = "event",
                UniversalPath = "universal",
                Port = 9876
            });
            PluginMain.Init(server);
            await server.StartServer(); 
        }
    }
}
