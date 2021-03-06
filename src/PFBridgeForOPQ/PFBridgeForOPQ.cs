﻿using System;
using System.Linq;
using Newtonsoft.Json;
using PFBridgeCore.EventArgs;
using Traceless.OPQSDK;
using Traceless.OPQSDK.Models.Api;
using Traceless.OPQSDK.Models.Event;
using Traceless.OPQSDK.Models.Msg;
using Traceless.OPQSDK.Plugin;
using static PFBridgeCore.APIs.EventsMap.IMEventsMap;
namespace PFBridgeForOPQ
{
    /// <summary>
    /// 示例插件 所有事件若不想使用可以直接去除事件代码
    /// </summary>
    public class PFBridgeForOPQ : BasePlugin
    {
        /// <summary>
        /// 插件名
        /// </summary>
        public override string pluginName => "PFBridgeForOPQ";

        /// <summary>
        /// 插件作者
        /// </summary>
        public override string pluginAuthor => "littlegao233";

        /// <summary>
        /// 插件APPID
        /// </summary>
        public override string AppId => "PFBridge.OPQ";

        /// <summary>
        /// 插件描述
        /// </summary>
        public override string PluginDescription => "PFBridgeForOPQ beta by gxh (github:https://github.com/littlegao233/PFBridge)";

        /// <summary>
        /// 插件优先级【越大越优先，优先级大的插件先被触发，若插件选择拦截消息，后面的插件将不会被触发群/私聊消息】
        /// </summary>
        public override int PluginPriority => 10;

        /// <summary>
        /// 群消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns>0不拦截 1拦截消息</returns>
        public override int GroupMsgProcess(GroupMsg _msg, long _CurrentQQ)
        {
            //msg.FromNickName
            //msg.FromGroupName
            try
            {
                var msg = _msg;
                var CurrentQQ = _CurrentQQ;
                void feedback(string s)
                {
                    try
                    {
                        Apis.SendGroupMsg(msg.FromGroupId, txt: $"[ATUSER({msg.FromUserId})]{s}");
                    }
                    catch
                    {
                        //System.Threading.Thread.Sleep(250);
                        //feedback(s);
                    }
                };
                PFBridgeCore.APIs.Events.IM.OnGroupMessage.Invoke(new GroupMessageEventsArgs(msg.FromGroupId, msg.FromUserId, msg.Content,
                      () => msg.FromGroupName,
                      () => msg.FromNickName,
                      () => msg.FromNickName,
                      () =>
                      {
                          var got = Apis.GetGroupUserList(msg.FromGroupId);
                          int i = got.FindIndex(x => x.MemberUin == msg.FromUserId);
                          if (i == -1) return 0;
                          else
                          {
                              long type = got[i].GroupAdmin;
                              if (type == 0)
                                  return 1;
                              if (type == 1)
                                  return 2;
                              if (type == 192)
                                  return 3;
                              return (int)type;
                          }
                      },
                      feedback,
                      () =>
                      {
                          return Decode.ParseMessage(msg.Content, msg.FromGroupId);
                      }
                  ));
            }
            catch (Exception ex) { PFBridgeCore.APIs.API.LogErr(ex); }
            return 0;
        }

        /// <summary>
        /// 私聊消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns>0不拦截 1拦截消息</returns>
        public override int FriendMsgProcess(FriendMsg msg, long CurrentQQ)
        {
            return 0;
        }

        /// <summary>
        /// QQ登陆成功事件
        /// </summary>
        /// <param name="msg"></param>
        public override void EventQQLogin(BaseEvent<QNetArgs> msg, long currentQQ)
        {
            Console.WriteLine($"EventQQLogin {currentQQ}\n" + JsonConvert.SerializeObject(msg));
        }

        /// <summary>
        /// 网络变化事件 网络波动引起当前链接 释放 随机8-15s会自动重连登陆 被t下线的QQ 不会在重连
        /// </summary>
        /// <param name="msg"></param>
        public override void EventFramNetChange(BaseEvent<QNetArgs> msg, long currentQQ)
        {
            Console.WriteLine($"EventQQNetChange {currentQQ}\n" + JsonConvert.SerializeObject(msg));
        }

        /// <summary>
        /// QQ离线事件 可能的原因(TX 踢号/异地登陆/冻结/被举报等 导致等Session失效)
        /// </summary>
        /// <param name="msg"></param>
        public override void EventQQOffline(BaseEvent<QNetArgs> msg, long currentQQ)
        {
            Console.WriteLine($"EventQQOffline {currentQQ}\n" + JsonConvert.SerializeObject(msg));
        }

        /// <summary>
        /// 加好友申请被同意/拒绝
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="currentQQ"></param>
        public override void EventQQFriendAddRet(BaseEvent<FriendAddReqRetArgs> msg, long currentQQ)
        {
            Console.WriteLine($"EventQQFriendAddRet {currentQQ}\n" + JsonConvert.SerializeObject(msg));
        }

        /// <summary>
        /// 主动删除了好友
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="currentQQ"></param>
        public override void EventQQFriendDelete(BaseEvent<FriendDeletArgs> msg, long currentQQ)
        {
            Console.WriteLine($"EventQQFriendDelete {currentQQ}\n" + JsonConvert.SerializeObject(msg));
        }

        /// <summary>
        /// 加好友成功后的通知
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="currentQQ"></param>
        public override void EventQQFriendAddPush(BaseEvent<FriendAddPushArgs> msg, long currentQQ)
        {
            Console.WriteLine($"EventQQFriendAddPush {currentQQ}\n" + JsonConvert.SerializeObject(msg));
        }

        /// <summary>
        /// 收到好友请求
        /// </summary>
        /// <param name="data"></param>
        /// <param name="currentQQ"></param>
        public override void EventQQFriendAddReq(BaseEvent<FriendAddReqArgs> msg, long currentQQ)
        {
            Console.WriteLine($"EventQQFriendAddReq {currentQQ}\n" + JsonConvert.SerializeObject(msg));
        }

        /// <summary>
        /// 退群成功
        /// </summary>
        /// <param name="data"></param>
        /// <param name="currentQQ"></param>
        public override void EventQQGroupExitSuc(BaseEvent<GroupExitSucArgs> msg, long currentQQ)
        {
            Console.WriteLine($"EventQQGroupExitSuc {currentQQ}\n" + JsonConvert.SerializeObject(msg));
        }

        /// <summary>
        /// 好友消息撤回
        /// </summary>
        /// <param name="data"></param>
        /// <param name="currentQQ"></param>
        public override void EventQQFriendRevoke(BaseEvent<FriendRevokeArgs> msg, long currentQQ)
        {
            Console.WriteLine($"EventQQFriendRevoke {currentQQ}\n" + JsonConvert.SerializeObject(msg));
        }

        /// <summary>
        /// 群禁言
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="currentQQ"></param>
        public override void EventQQGroupShut(BaseEvent<GroupShutArgs> msg, long currentQQ)
        {
            Console.WriteLine($"EventQQGroupShut {currentQQ}\n" + JsonConvert.SerializeObject(msg));
        }

        /// <summary>
        /// 群撤回
        /// </summary>
        /// <param name="data"></param>
        /// <param name="currentQQ"></param>
        public override void EventQQGroupRevoke(BaseEvent<GroupRevokeArgs> msg, long currentQQ)
        {
            Console.WriteLine($"EventQQGroupRevoke {currentQQ}\n" + JsonConvert.SerializeObject(msg));
        }

        /// <summary>
        /// 群头衔变更
        /// </summary>
        /// <param name="data"></param>
        /// <param name="currentQQ"></param>
        public override void EventQQGroupTitleChange(BaseEvent<GroupTitleChangeArgs> msg, long currentQQ)
        {
            Console.WriteLine($"EventQQGroupTitleChange {currentQQ}\n" + JsonConvert.SerializeObject(msg));
        }

        /// <summary>
        /// 加群相关，加群请求、成功入群
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="currentQQ"></param>
        public override void EventQQGroupJoin(BaseEvent<GroupJoinReqArgs> msg, long currentQQ)
        {
            Console.WriteLine($"EventQQGroupJoin {currentQQ}\n" + JsonConvert.SerializeObject(msg));
        }

        /// <summary>
        /// 群管理变更-机器人是不是管理员都能收到此群管变更事件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="currentQQ"></param>
        public override void EventQQGroupAdminChange(BaseEvent<GroupAdminChangeArgs> msg, long currentQQ)
        {
            Console.WriteLine($"EventQQGroupAdminChange {currentQQ}\n" + JsonConvert.SerializeObject(msg));
        }

        /// <summary>
        /// 有人退群-无论机器人是不是管理员 群里任意成员 都能收到 此退群事件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="currentQQ"></param>
        public override void EventQQGroupExitPush(BaseEvent<GroupExitPushArgs> msg, long currentQQ)
        {
            Console.WriteLine($"EventQQGroupExitPush {currentQQ}\n" + JsonConvert.SerializeObject(msg));
        }

        /// <summary>
        /// 加群成功
        /// </summary>
        /// <param name="data"></param>
        /// <param name="currentQQ"></param>
        public override void EventQQGroupJoinSuc(BaseEvent<GroupJoinSucArgs> msg, long currentQQ)
        {
            Console.WriteLine($"EventQQGroupJoinSuc {currentQQ}\n" + JsonConvert.SerializeObject(msg));
        }

        /// <summary>
        /// 收到群邀请
        /// </summary>
        /// <param name="data"></param>
        /// <param name="currentQQ"></param>
        public override void EventQQGroupInvite(BaseEvent<GroupInviteArgs> msg, long currentQQ)
        {
            Console.WriteLine($"EventQQGroupInvite {currentQQ}\n" + JsonConvert.SerializeObject(msg));
        }

        /// <summary>
        /// 有人退群
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="currentQQ"></param>
        public override void EventQQGroupExit(BaseEvent<GroupInviteArgs> msg, long currentQQ)
        {
            Console.WriteLine($"EventQQGroupExit {currentQQ}\n" + JsonConvert.SerializeObject(msg));
        }

        /// <summary>
        /// 有人提交加群申请
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="currentQQ"></param>
        public override void EventQQGroupJoinSub(BaseEvent<GroupInviteArgs> msg, long currentQQ)
        {
            Console.WriteLine($"EventQQGroupJoinSub {currentQQ}\n" + JsonConvert.SerializeObject(msg));
        }

        /// <summary>
        /// 加群申请被批准
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="currentQQ"></param>
        public override void EventQQGroupJoinSubAgree(BaseEvent<GroupInviteArgs> msg, long currentQQ)
        {
            Console.WriteLine($"EventQQGroupJoinSub {currentQQ}\n" + JsonConvert.SerializeObject(msg));
        }

        /// <summary>
        /// 插件初始化
        /// </summary>
        /// <param name="currentQQ"></param>
        public override void PluginInit(long currentQQ)
        {
            base.PluginInit(currentQQ);
            try { PFBridgeCore.Main.Init(new API());/*主体注入*/} catch (Exception ex) { Console.WriteLine("主入口注入失败：" + ex); }
        }
    }
}