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

        public void SendGroupMessage(string TargetGroup, string Message)
        {
            Apis.SendGroupMsg(long.Parse(TargetGroup), txt:Message);
        }

        public void SendPrivateMessageFromGroup(string TargetGroup, string QQid, string Message)
        {
            Apis.SendFriendMsg(long.Parse(QQid),Message);
        }
    }
}