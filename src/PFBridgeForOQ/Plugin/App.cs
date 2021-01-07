using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBoxs.Sdk.Cqp.EventArgs;
using PFBridgeCore.EventArgs;

namespace PFBridgeForOQ.Plugin
{
    internal static class App
    {
        private static bool hasStarted = false;
        internal static void OnStartup(long qq)
        {
            if (!hasStarted)
            {
                PFBridgeCore.Main.Init(new API(qq));
                hasStarted = true;
            }
        }
        /// <summary>
        /// 处理消息接收事件。
        /// </summary>
        internal static void OnMessageReceived(CqGroupMessageEventArgs e)
        {
            try { PFBridgeCore.QQAPI.Events.OnGroupMessage.Invoke(new GroupMessageEventsArgs(e.FromGroup, e.FromQQ, e.Message)); }
            catch (Exception ex) { PFBridgeCore.QQAPI.API.LogErr(ex); }
        }
    }
}
