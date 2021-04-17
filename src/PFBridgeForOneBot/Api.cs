using Console = Colorful.Console;
using PFBridgeCore;
using Sora.Entities.Base;
using System.Collections.Generic;
using System.Drawing;
using PFBridgeCore.EventArgs;
using Colorful;
using System.Text.RegularExpressions;

namespace PFBridgeForOneBot
{
    internal class API : IBridgeIMBase
    {
        internal API(SoraApi _api)
        {
            BaseApi = _api;
        }
        internal static SoraApi BaseApi = null;
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
            Console.WriteLineFormatted("{0}{1}{2}{3}{4}{5}{6}", Color.White, new Formatter[] {
                new Formatter("[", Color.PeachPuff),
                new Formatter(System.DateTime.Now.ToString("G"), Color.LightGreen),
                new Formatter("]", Color.PeachPuff),
                new Formatter("[", Color.Orange),
                new Formatter("INFO", Color.Cyan),
                new Formatter("]", Color.Orange),
                new Formatter(Message.ToString(), Color.LightPink)
            });
        }
        public void LogErr(object Message)
        {
            Console.WriteLineFormatted("{0}{1}{2}{3}{4}{5}{6}", Color.White, new Formatter[] {
                new Formatter("[", Color.DarkKhaki),
                new Formatter(System.DateTime.Now.ToString("G"), Color.LightBlue),
                new Formatter("]", Color.DarkKhaki),
                new Formatter("[", Color.Yellow),
                new Formatter("Error", Color.PaleVioletRed),
                new Formatter("]", Color.Yellow),
                new Formatter(Message.ToString(), Color.OrangeRed)
            });
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
        public IParseMessageFormat ParseMessageFormat { get; set; } = new PFBridgeCore.Model.DefaultParseFormat();
    }
}
