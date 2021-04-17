using PFBridgeCore;
using System.Collections.Generic;
using IBoxs.Core.App;
namespace PFBridgeForOQ.Plugin
{
    internal class API : IBridgeIMBase
    {
        public API(long qq) => RobotQQ = qq;
        public readonly long RobotQQ;
        public string PluginDataPath
        {
            get
            {
                if (!System.IO.Directory.Exists(Common.AppDirectory)) System.IO.Directory.CreateDirectory(Common.AppDirectory);
                return Common.AppDirectory;
            }
        }
        public void Log(object Message)
        {
            Common.CqApi.OutLog(Message.ToString());
        }
        public void LogErr(object Message)
        {
            //System.Windows.Forms.MessageBox.Show(Message.ToString());
            Common.CqApi.OutLog("[Error]" + Message.ToString());
        }
        public void SendGroupMessage(long TargetGroup, string Message)
        {
            Common.CqApi.SendGroupMessage(RobotQQ,  TargetGroup, Message);
        }
        public void SendPrivateMessageFromGroup(long TargetGroup, long QQid, string Message)
        {
            Common.CqApi.SendGroupPrivateMessage(RobotQQ, TargetGroup,  QQid, Message);
        }
        public IParseMessageFormat ParseMessageFormat { get; set; } = new PFBridgeCore.Model.DefaultParseFormat();

    }
}