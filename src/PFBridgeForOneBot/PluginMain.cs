﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static PFBridgeCore.APIs.EventsMap.IMEventsMap;
using PFBridgeCore;
using Sora.OnebotModel;
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
        internal static void Init(Sora.Interfaces.ISoraService server)
        {
            server.Event.OnFileUpload += (sender, eventArgs) =>
            {
                try
                {
                    Sora.EventArgs.SoraEvent.FileUploadEventArgs e = eventArgs;
                    //e.SoraApi.GetGroupMemberInfo
                    PFBridgeCore.APIs.Events.IM.OnGroupMessage.Invoke(new GroupMessageEventsArgs(e.SourceGroup.Id, e.Sender.Id, $"群文件：{ e.FileInfo.Name }[{ e.FileInfo.Size}]",
                            () =>
                            {
                                var get = e.SourceGroup.GetGroupInfo().AsTask();
                                get.Wait();
                                return get.Result.groupInfo.GroupName;
                            },
                          () =>
                          {
                              var info = e.SourceGroup.GetGroupMemberInfo(e.Sender.Id).Result.memberInfo;
                              return info.Nick;
                          },
                          () => CodeEx.GetMemberCard(e.SourceGroup.Id, e.Sender.Id),
                          () =>
                          {
                              var info = e.SourceGroup.GetGroupMemberInfo(e.Sender.Id).Result.memberInfo;
                              return (int)info.Role + 1;
                          },
                          //     成员        Member = 0,
                          //     管理员        Admin = 1,
                          //     群主        Owner = 2
                          s =>
                          {
                              e.SourceGroup.SendGroupMessage(  Sora.Entities.MessageElement.CQCodes.CQAt(e.Sender.Id),Sora.Entities.MessageElement.CQCodes.CQText(s));
                              //e.SourceGroup.SendGroupMessage(new Sora.Entities.MessageBody(new List<Sora.Entities.MessageElement.CQCode> {
                              //                                                                  new Sora.Entities.MessageElement.CQCode(){MessageType=Sora.Enumeration.CQType.At   },
                              //                                                                  new Sora.Entities.MessageElement.CQCode(){  }
                          },
                          () => string.Format(PFBridgeCore.APIs.API.ParseMessageFormat.File, $"{e.FileInfo.Name}[{e.FileInfo.Size}]")
                      ));
                }
                catch (Exception ex) { PFBridgeCore.APIs.API.LogErr(ex); }
                return new ValueTask();
            };
            server.Event.OnGroupMessage += (sender, eventArgs) =>
            {
                try
                {
                    Sora.EventArgs.SoraEvent.GroupMessageEventArgs e = eventArgs;
                    //e.SoraApi.GetGroupMemberInfo
                    PFBridgeCore.APIs.Events.IM.OnGroupMessage.Invoke(new GroupMessageEventsArgs(e.SourceGroup.Id, e.Sender.Id, CodeEx.CQDeCode(e.Message.RawText),
                          () =>
                          {
                              var get = e.SourceGroup.GetGroupInfo().AsTask();
                              get.Wait();
                              return get.Result.groupInfo.GroupName;
                          },
                          () => e.SenderInfo.Nick,
                          () => string.IsNullOrEmpty(e.SenderInfo.Card) ? e.SenderInfo.Nick : e.SenderInfo.Card,
                          () => (int)e.SenderInfo.Role + 1,
                          //     成员        Member = 0,
                          //     管理员        Admin = 1,
                          //     群主        Owner = 2
                          //s => e.SourceGroup.SendGroupMessage($"[CQ:reply,id={e.Message.MessageId}]{s}"),
                          //s => e.SourceGroup.SendGroupMessage($"[CQ:at,qq={e.Sender.Id}]{s}"  ),
                          s => e.SourceGroup.SendGroupMessage(Sora.Entities.MessageElement.CQCodes.CQAt(e.Sender.Id), Sora.Entities.MessageElement.CQCodes.CQText(s)),
                          () => CodeEx.ParseMessage(e.Message.RawText, e.SourceGroup.Id)
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
    }
}
