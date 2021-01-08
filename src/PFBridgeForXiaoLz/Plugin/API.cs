using PFBridgeCore;
using System.Collections.Generic;
using static SDK.Common;
namespace PFBridgeForXiaoLz.Plugin
{
    internal class API : IBridgeQQBase
    {
        public string PluginDataPath
        {
            get
            {
                string path = xlzAPI.GetPluginDataDirectoryEvent();
                if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
                return path;
            }
        }
        public void Log(object Message)
        {
            xlzAPI.OutLog(Message.ToString(), 0xa179ca, 0xeeeeee);
        }
        public void LogErr(object Message)
        {
            xlzAPI.OutLog1(Message.ToString(), System.Drawing.Color.Red, System.Drawing.Color.LightGoldenrodYellow);
#if DEBUG
            System.Windows.Forms.MessageBox.Show(Message.ToString(), "ERROR");
#endif
        }
        public void SendGroupMessage(long TargetGroup, string Message)
        {
            App.ForEachQQ(qqid => xlzAPI.SendGroupMessage(qqid, TargetGroup, Message));
            //.ForEach(l => SendGroupMsg(PInvoke.plugin_key, l, group, Message, false));
            //xlzAPI.SendGroupMessage(TargetGroup, Message);
        }
        public void SendPrivateMessageFromGroup(long TargetGroup, long QQid, string Message)
        {
            App.ForEachQQ(qqid => xlzAPI.SendGroupTemporaryMessageEvent(qqid, TargetGroup, QQid, Message));
            //xlzAPI.sendg(TargetGroup, QQid, Message);
        }
    }
}