using System;
using System.Threading.Tasks;
using Sora.Server;
using System.IO;
using Newtonsoft.Json;
using Sora.Extensions;
using Alba.CsConsoleFormat;
using Console = Colorful.Console;
namespace PFBridgeForOneBot
{
    class Program
    {
        static void WriteLine(string content)
        {
            Console.WriteLine($"[{DateTime.Now:G}][INFO][PFBridgeForOneBot]" + content,System.Drawing.Color.Yellow);
        }
        static void WriteLineErr(string content)
        {
            Console.WriteLine($"[{DateTime.Now:G}][Error][PFBridgeForOneBot]" + content);
        }
        static string ConfigPath { get { return Path.GetFullPath("config.json"); } }
        private static ServerConfig _ConfigData = null;
        static ServerConfig ConfigData
        {
            get
            {
                if (_ConfigData is null)
                {
                    if (File.Exists(ConfigPath))
                    {
                        WriteLine("正在读取\"config.json\"...");
                        try { _ConfigData = JsonConvert.DeserializeObject<ServerConfig>(File.ReadAllText(ConfigPath)); }
                        catch (Exception ex)
                        {
                            WriteLineErr("配置文件读取错误！请检查\"config.json\"填写是否规范！\n信息" + ex.ToString());
                            System.Threading.Thread.Sleep(5000);
                            throw;
                        }
                    }
                    else
                        ConfigData = new ServerConfig();
                }
                return _ConfigData;
            }
            set
            {
                File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(value, Formatting.Indented));
                WriteLine("已输出默认配置文件：\"config.json\"\t请自行修改！");
                _ConfigData = value;
            }
        }
        static async Task Main(string[] args)
        {
            SoraWSServer server = new SoraWSServer(ConfigData);
            PluginMain.Init(server); 
            WriteLine("QQ端参考配置方法：");
            var doc = new Document(
                       new Dock()
                       {
                           Background = ConsoleColor.DarkGray,
                           Margin = new Thickness(1),
                           Children = {
                           new  Border()
                           {
                                Background=ConsoleColor.Blue,
                               Margin=new Thickness(1),
                                Stroke=new LineThickness(LineWidth.Single),
                                Align=Align.Stretch,
                                Children = {
                                    new Span("github.com/Mrs4s/go-cqhttp") { Color = ConsoleColor.Gray  },
                                    new Separator()  ,
                                    new Span(@$"......
ws_reverse_servers: [
    {{
        enabled: true
        reverse_url: ws://127.0.0.1:{ConfigData.Port}/{ConfigData.UniversalPath}
        reverse_api_url: ws://127.0.0.1:{ConfigData.Port}/{ConfigData.ApiPath}
        reverse_event_url: ws://127.0.0.1:{ConfigData.Port}/{ConfigData.EventPath}
        reverse_reconnect_interval: 3000
    }}
]
......
post_message_format: array
......") { Color = ConsoleColor.Yellow }
                               }
                            } ,
                           new  Border()
                           {
                               Background=ConsoleColor.Blue,
                               Align=Align.Right,
                               Stroke=new LineThickness(LineWidth.Single),
                               Margin=new Thickness(1),
                               Children = {
                                    new Span("github.com/yyuueexxiinngg/onebot-kotlin") { Color = ConsoleColor.Gray },
                                    new Separator()  ,
                                    new Span(@$"......
    heartbeat: 
      enable: true
......
    ws_reverse: 
      - enable: true
        postMessageFormat: array
        reverseHost: 127.0.0.1
        reversePort: {ConfigData.Port}
        accessToken: ''
        reversePath: '/{ConfigData.UniversalPath}'
        reverseApiPath: '/{ConfigData.ApiPath}'
        reverseEventPath: '/{ConfigData.EventPath}'
......") { Color = ConsoleColor.Yellow }
                               }
                            }
                           }
                       }
                   );
            ConsoleRenderer.RenderDocument(doc);
            await server.StartServer().RunCatch();
        }
    }
}
