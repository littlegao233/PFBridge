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
            RefreshQQList();
        }
        private static byte counter = 254;
        /// <summary>
        /// 处理消息接收事件。
        /// </summary>
        internal static void OnMessageReceived(ref SDK.Events.GroupMessageEvent e)
        {
            if (++counter == byte.MaxValue) { counter = byte.MinValue; App.RefreshQQList(); }
            try { PFBridgeCore.QQAPI.Events.OnGroupMessage.Invoke(new GroupMessageEventsArgs(e.MessageGroupQQ, e.SenderQQ, e.MessageContent)); }
            catch (Exception ex) { PFBridgeCore.QQAPI.API.LogErr(ex); App.RefreshQQList(); }
        }
        private static List<long> QQList = new List<long>();
        internal static void RefreshQQList()
        {
            var obj = Newtonsoft.Json.Linq.JObject.Parse(SDK.Common.xlzAPI.GetThisQQ())["QQlist"];
            QQList = obj.ToList().ConvertAll(l => long.Parse(((Newtonsoft.Json.Linq.JProperty)l).Name));
        }
        internal static void ForEachQQ(Action<long> action)
        {
            QQList.ForEach(action);
        }
    }
}
