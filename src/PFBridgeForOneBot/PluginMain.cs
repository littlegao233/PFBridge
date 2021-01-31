using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sora.Server;
namespace PFBridgeForOneBot
{
    class PluginMain
    {
        internal static void Init(SoraWSServer server)
        {
            server.Event.OnGroupMessage += async (sender, eventArgs) =>
            {
                await eventArgs.SourceGroup.SendGroupMessage(eventArgs.Message.MessageList);
            };
        }
    }
}
