using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFBridgeCore.EventArgs;
using static PFBridgeCore.APIs.EventsMap.QQEventsMap;
namespace PFBridgeForER.Plugin
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
            //RefreshQQList();
        }
        private static byte counter = 254;
        /// <summary>
        /// 处理消息接收事件。
        /// </summary>
        internal static void OnMessageReceived(string group , string sender , string message,string _robotQQ )
        {
            //if (++counter == byte.MaxValue) { counter = byte.MinValue; App.RefreshQQList(); }
            try
            {
                PFBridgeCore.APIs.Events.QQ.OnGroupMessage.Invoke(new GroupMessageEventsArgs(long.Parse(group), long.Parse(sender), message,
                  () => IRQQ.CSharp.ERApi.GetGroupName(_robotQQ,group),
                  () => IRQQ.CSharp.ERApi.GetNickName(_robotQQ, sender),
                  () => IRQQ.CSharp.ERApi.GetMemberCard(_robotQQ,group, sender),
                  (s) => IRQQ.CSharp.ERApi.SendGroupMessage(group, $"[IR:at={sender}]{s}" )//https://gitee.com/jiguang_aurora/CleverQQ-SDK/wikis/%E5%8F%98%E9%87%8F%E5%88%97%E8%A1%A8?sort_id=1516257
              ));
            }
            catch (Exception ex) { PFBridgeCore.APIs.API.LogErr(ex); /*App.RefreshQQList();*/ }
        }
        //private static List<long> QQList = new List<long>();
        //internal static void RefreshQQList()
        //{
        //    var obj = Newtonsoft.Json.Linq.JObject.Parse(SDK.Common.xlzAPI.GetThisQQ())["QQlist"];
        //    QQList = obj.ToList().ConvertAll(l => long.Parse(((Newtonsoft.Json.Linq.JProperty)l).Name));
        //}
        //internal static void ForEachQQ(Action<long> action)
        //{
        //    QQList.ForEach(action);
        //}
    }
}
