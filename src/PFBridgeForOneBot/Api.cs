using Console = Colorful.Console;
using PFBridgeCore;
using Sora.Entities.Base;
using System.Collections.Generic;
using System.Drawing;
namespace PFBridgeForOneBot
{
    internal class API : IBridgeQQBase
    {
        internal API(SoraApi _api)
        {
            BaseApi = _api;
        }
        internal SoraApi BaseApi = null;
        public string PluginDataPath
        {
            get
            {
                string p = System.IO.Path.GetFullPath("PFBridgeForOneBot");
                if (!System.IO.Directory.Exists(p)) System.IO.Directory.CreateDirectory(p);
                return p;
            }
        }
        public void Log(object Message)
        {
            Console.Write("[", Color.DarkKhaki);
            Console.Write(System.DateTime.Now.ToString("G"), Color.LightGreen);
            Console.Write("][", Color.DarkKhaki);
            Console.Write("INFO", Color.Cyan);
            Console.Write("] ", Color.DarkKhaki);
            Console.WriteLine(Message.ToString(), Color.LightPink);
        }

        public void LogErr(object Message)
        {
            Console.Write("[", Color.DarkKhaki);
            Console.Write(System.DateTime.Now.ToString("G"), Color.LightGreen);
            Console.Write("][", Color.DarkKhaki);
            Console.Write("Error", Color.PaleVioletRed);
            Console.Write("] ", Color.DarkKhaki);
            Console.WriteLine(Message.ToString(), Color.OrangeRed);
        }

        public async void SendGroupMessage(long TargetGroup, string Message)
        {
            try
            {
                await BaseApi.SendGroupMessage(TargetGroup, Message);
            }
            catch (System.Exception ex) { Console.WriteLine(ex.ToString(), System.ConsoleColor.Red); }
        }

        public async void SendPrivateMessageFromGroup(long TargetGroup, long QQid, string Message)
        {
            try
            {
                await BaseApi.SendPrivateMessage(QQid, Message);
            }
            catch (System.Exception ex) { Console.WriteLine(ex.ToString(), System.ConsoleColor.Red); }
        }
    }
}
