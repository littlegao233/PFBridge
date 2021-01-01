using PFBridgeCore;
 using System.Collections.Generic;
using Traceless.OPQSDK;

namespace PFBridgeToOPQ
{
     internal class API : IBridgeQQBase
    {
        public string PluginDataPath
        {
            get
            {
                return Apis.GetPluginDataDic("PFBridgeToOPQ");
            }
        }
        public void Log(object Message)
        {
            System.Console.WriteLine(Message.ToString());
        }

        public void LogErr(object Message)
        {
            System.Console.WriteLine(Message.ToString());
        }

        public void SendGroupMessage(long TargetGroup, string Message)
        {
            Apis.SendGroupMsg(TargetGroup, txt:Message);
        }

        public void SendPrivateMessageFromGroup(long TargetGroup, long QQid, string Message)
        {
            Apis.SendFriendMsg(QQid,Message);
        }
    }
}