using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sora.Server;
using static PFBridgeCore.APIs.EventsMap.QQEventsMap;

namespace PFBridgeForOneBot
{
    class PluginMain
    {
        private static bool hasStarted = false;
        internal static void AppStart(Sora.Entities.Base.SoraApi api)
        {
            if (hasStarted) return;
            PFBridgeCore.Main.Init(new API(api));
            hasStarted = true;
        }
        internal static void Init(SoraWSServer server)
        {
            server.Event.OnGroupMessage += (sender, eventArgs) =>
          {
              try
              {
                  PFBridgeCore.APIs.Events.QQ.OnGroupMessage.Invoke(new GroupMessageEventsArgs(eventArgs.SourceGroup.Id, eventArgs.Sender.Id, CQDeCode(eventArgs.Message.RawText),
                        () => eventArgs.SourceGroup.GetGroupInfo().Result.groupInfo.GroupName,
                        () => eventArgs.SenderInfo.Nick,
                        () => eventArgs.SenderInfo.Card,
                        () => (int)eventArgs.SenderInfo.Role + 1,
                        /*
                            //
        // 摘要:
        //     成员
        Member = 0,
        //
        // 摘要:
        //     管理员
        Admin = 1,
        //
        // 摘要:
        //     群主
        Owner = 2
                         */
                        s => eventArgs.Reply(s)
                    ));
              }
              catch (Exception ex) { PFBridgeCore.APIs.API.LogErr(ex); }
              return new ValueTask();
          };
            server.Event.OnClientConnect += (sender, eventArgs) =>
            {
                AppStart(eventArgs.SoraApi);
                return new ValueTask();
            };
            //server.Event.c += async (sender, eventArgs) =>
            //{
            //    await eventArgs.SourceGroup.SendGroupMessage(eventArgs.Message.MessageList);
            //};
        }
        internal static string CQDeCode(string source)
        {
            if (source == null) return string.Empty;
            var builder = new StringBuilder(source);
            builder = builder.Replace("&#91;", "[");
            builder = builder.Replace("&#93;", "]");
            builder = builder.Replace("&#44;", ",");
            builder = builder.Replace("&amp;", "&");
            return builder.ToString();
        }
    }
}
