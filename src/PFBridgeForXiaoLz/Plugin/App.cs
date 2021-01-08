using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFBridgeCore.EventArgs;

namespace PFBridgeForXiaoLz.Plugin
{
    internal static class App
    {
        private static bool hasStarted = false;
        internal static void OnStartup()
        {
            if (!hasStarted)
            {
                PFBridgeCore.Main.Init(new API());
                hasStarted = true;
            }
        }
        /// <summary>
        /// 处理消息接收事件。
        /// </summary>
        internal static void OnMessageReceived(ref SDK.Events.GroupMessageEvent e)
        {
            try { PFBridgeCore.QQAPI.Events.OnGroupMessage.Invoke(new GroupMessageEventsArgs(e.MessageGroupQQ, e.SenderQQ, e.MessageContent)); }
            catch (Exception ex) { PFBridgeCore.QQAPI.API.LogErr(ex); }
        }
    }
}
