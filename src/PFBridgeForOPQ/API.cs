using PFBridgeCore;
using System.Collections.Generic;
using Traceless.OPQSDK;

namespace PFBridgeForOPQ
{
    internal class API : IBridgeQQBase
    {
        public string PluginDataPath
        {
            get
            {
                string path = Apis.GetPluginDataDic("PFBridgeForOPQ");
                if (!System.IO.File.Exists(path)) System.IO.Directory.CreateDirectory(path);
                //System.IO.Directory.Exists()
                return path;
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
            try { Apis.SendGroupMsg(TargetGroup, txt: Message); }
            catch (System.Exception ex) { System.Console.WriteLine(ex.ToString()); }
        }

        public void SendPrivateMessageFromGroup(long TargetGroup, long QQid, string Message)
        {
            try { Apis.SendFriendMsg(QQid, Message); }
            catch (System.Exception ex) { System.Console.WriteLine(ex.ToString()); }
        }
    }
}