using PFBridgeCore;
using System.Collections.Generic;
using IRQQ.CSharp;
namespace PFBridgeForER.Plugin
{
    internal class API : IBridgeQQBase
    {
        public string PluginDataPath
        {
            get
            {
                string path = ERApi.PluginDataPath;
                if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
                return path;
            }
        }
        public void Log(object Message)
        {
            ERApi.Log(Message.ToString() );
        }
        public void LogErr(object Message)
        {
            ERApi.Log("[ERROR]"+Message.ToString() );
        }
        public void SendGroupMessage(long TargetGroup, string Message)
        {
            ERApi.SendGroupMessage(TargetGroup, Message);
        }
        public void SendPrivateMessageFromGroup(long TargetGroup, long QQid, string Message)
        {
            ERApi.SendPrivateMessageFromGroup(TargetGroup, QQid, Message);
        }
    }
}