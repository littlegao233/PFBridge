using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFBridgeCore.EventArgs;
using static PFBridgeCore.APIs.EventsMap.QQEventsMap;

namespace PFBridgeForXiaoLz.Plugin
{
    internal static class App
    {
        private static bool hasStarted = false;
        internal static void OnStartup()
        {
            if (!hasStarted)
            {
                hasStarted = true;
                PFBridgeCore.Main.Init(new API());
            }
            RefreshQQList();
        }
        private static byte counter = 254;
        /// <summary>
        /// 处理消息接收事件。
        /// </summary>
        internal static void OnMessageReceived(SDK.Events.GroupMessageEvent _e)
        {
            if (++counter == byte.MaxValue) { counter = byte.MinValue; App.RefreshQQList(); }
            try
            {
                var e = _e;
                PFBridgeCore.APIs.Events.QQ.OnGroupMessage.Invoke(new GroupMessageEventsArgs(e.MessageGroupQQ, e.SenderQQ, e.MessageContent,
                  () => e.SourceGroupName,
                  () => e.SenderNickname,// SDK.Common.xlzAPI.GetOneGroupMemberInfo(e.ThisQQ, e.MessageGroupQQ, e.SenderQQ).NickName
                  () => SDK.Common.xlzAPI.GetOneGroupMemberInfo(e.ThisQQ, e.MessageGroupQQ, e.SenderQQ).GroupCardName,
                  () => (int)SDK.Common.xlzAPI.GetOneGroupMemberInfo(e.ThisQQ, e.MessageGroupQQ, e.SenderQQ).groupPosition,
                  (s) => SDK.Common.xlzAPI.SendGroupMessage(e.ThisQQ, e.MessageGroupQQ, SDK.Common.xlzAPI.GetAt(e.SenderQQ) + s)
              ));
            }
            catch (Exception ex) { PFBridgeCore.APIs.API.LogErr(ex); App.RefreshQQList(); }
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
