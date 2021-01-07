using PFBridgeCore;
using System.Collections.Generic;
using static XiaolzCSharp.API;
namespace PFBridgeForOQ.Plugin
{
    internal class API : IBridgeQQBase
    {
        public string PluginDataPath
        {
            get
            {
                string path = GetPluginDataDirectory();
                if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
                return path;
            }
        }
        public void Log(object Message)
        {
            OutPutLogCall(Message.ToString(),0,0);
        }
        public void LogErr(object Message)
        {
            OutPutLogCall(Message.ToString(), 0, 255); 
        }
        public void SendGroupMessage(long TargetGroup, string Message)
        {
            SendGroupMsgCall(TargetGroup, Message);
        }
        public void SendPrivateMessageFromGroup(long TargetGroup, long QQid, string Message)
        {
            SendPrivateMsgCall(TargetGroup, QQid, Message);
        }
    }
}