using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBoxs.Sdk.Cqp.EventArgs;
using PFBridgeCore.EventArgs;
using static PFBridgeCore.APIs.EventsMap.QQEventsMap;

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
            //IBoxs.Core.App.Common.CqApi.GetMemberInfo(e.FromGroup)
            try
            {
                PFBridgeCore.APIs.Events.QQ.OnGroupMessage.Invoke(new GroupMessageEventsArgs(e.FromGroup, e.FromQQ, e.Message,
                    () => IBoxs.Core.App.Common.CqApi.GetGroupInfo(e.RobotQQ, e.FromGroup).Name,
                    () => IBoxs.Core.App.Common.CqApi.GetQQNick(e.RobotQQ, e.FromQQ),
                    () => IBoxs.Core.App.Common.CqApi.GetMemberInfo(e.RobotQQ, e.FromGroup, e.FromQQ).Card,
                    () => (int)IBoxs.Core.App.Common.CqApi.GetMemberInfo(e.RobotQQ, e.FromGroup, e.FromQQ).PermitType,
                    (s) => IBoxs.Core.App.Common.CqApi.SendGroupMessage(e.RobotQQ, e.FromGroup, IBoxs.Core.App.Common.CqApi.CqCode_At(e.FromQQ)+s)
              ));
            }
            catch (Exception ex) { PFBridgeCore.APIs.API.LogErr(ex); }
        }
    }
}
