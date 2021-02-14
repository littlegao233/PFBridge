using   Console = Colorful.Console;
using PFBridgeCore;
using System.Collections.Generic;
using Traceless.OPQSDK;
using System.Drawing;
namespace PFBridgeForOPQ
{
    internal class API : IBridgeQQBase
    {
        public string PluginDataPath
        {
            get
            {
                string path = Apis.GetPluginDataDic("PFBridgeForOPQ");
                if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
                return path;
            }
        }

        public IParseMessageFormat ParseMessageFormat { get; set; } = new PFBridgeCore.Model.DefaultParseFormat();

        public void Log(object Message)
        {
            Console.Write("[",Color.DarkKhaki);
            Console.Write(System.DateTime.Now.ToString("HH:mm:ss"), Color.LightGreen);
            Console.Write(" INFO", Color.Cyan);
            Console.Write("] ", Color.DarkKhaki);
            Console.WriteLine(Message.ToString(), Color.LightPink);
        }

        public void LogErr(object Message)
        {
            Console.Write("[", Color.DarkKhaki);
            Console.Write(System.DateTime.Now.ToString("HH:mm:ss"), Color.LightGreen);
            Console.Write(" Error", Color.PaleVioletRed);
            Console.Write("] ", Color.DarkKhaki);
            Console.WriteLine(Message.ToString(), Color.OrangeRed);
        }

        public void SendGroupMessage(long TargetGroup, string Message)
        {
            try { Apis.SendGroupMsg(TargetGroup, txt: Message); }
            catch (System.Exception ex) { Console.WriteLine(ex.ToString(), System.ConsoleColor.Red); }
        }

        public void SendPrivateMessageFromGroup(long TargetGroup, long QQid, string Message)
        {
            try { Apis.SendFriendMsg(QQid, Message); }
            catch (System.Exception ex) { Console.WriteLine(ex.ToString(), System.ConsoleColor.Red); }
        }
    }
}