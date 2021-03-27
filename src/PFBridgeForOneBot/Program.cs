using System;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Alba.CsConsoleFormat;
using Console = Colorful.Console;
using Sora.OnebotModel;
using Sora.Net;
using YukariToolBox.Extensions;

namespace PFBridgeForOneBot
{

    class Program
    {
        static void WriteLine(string content)
        {
            Console.WriteLine($"[{DateTime.Now:G}][INFO][PFBridgeForOneBot]" + content, System.Drawing.Color.Yellow);
        }
        static void WriteLineErr(string content)
        {
            Console.WriteLine($"[{DateTime.Now:G}][Error][PFBridgeForOneBot]" + content);
        }

        static string ConfigPath { get { return Path.GetFullPath("config.json"); } }
        private static ServerConfigModel _ConfigData = null;
        static ServerConfigModel ConfigData
        {
            get
            {
                if (_ConfigData is null)
                {
                    if (File.Exists(ConfigPath))
                    {
                        WriteLine("正在读取\"config.json\"...");
                        try
                        {
                            _ConfigData = JsonConvert.DeserializeObject<ServerConfigModel>(File.ReadAllText(ConfigPath));
                            File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(_ConfigData, Formatting.Indented));
                        }
                        catch (Exception ex)
                        {
                            WriteLineErr("配置文件读取错误！请检查\"config.json\"填写是否规范！\n信息" + ex.ToString());
                            System.Threading.Thread.Sleep(5000);
                            throw;
                        }
                    }
                    else
                        ConfigData = new ServerConfigModel();
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
            var service = SoraServiceFactory.CreateInstance(ConfigData.GetServerConfig());
            PluginMain.Init(service);
            WriteLine("QQ端参考配置方法：");
            Span EditText(string content) => new Span(content) { Background = ConsoleColor.Magenta, Color = ConsoleColor.Gray };
            Span BgText(string content) => new Span(content) { Color = ConsoleColor.Yellow };
            Span SeptTxt = new Span("......") { Color = ConsoleColor.Cyan };
            Span NewLine = new Span("\n");
            try
            {
                var doc = new Document(
                                  new Grid()
                                  {
                                      TextAlign = TextAlign.Left,
                                      Columns = {
                                new Column() {Width=new GridLength(1,GridUnit.Star)},
                                new Column() {Width=new GridLength(1,GridUnit.Auto)},
                                new Column() {Width=new GridLength(1,GridUnit.Star)}
                                       },
                                      Background = ConsoleColor.DarkGray,
                                      Children = {
                               new  Border()
                               {
                                    Background=ConsoleColor.Blue,
                                    Margin=new Thickness(1,0,0,0),
                                    Align=Align.Stretch,
                                    Children = {
                                       new  Border(new Span("github.com/Mrs4s/go-cqhttp"){Color=ConsoleColor.DarkBlue,Background=ConsoleColor.DarkCyan}){Align=Align.Center},
                                       new Separator(),
                                       SeptTxt,
                                       NewLine,BgText("ws_reverse_servers: ["),
                                       NewLine,BgText("    {"),
                                       NewLine,BgText("        enabled: "),EditText("true") ,
                                       NewLine,BgText("        reverse_url: ws://127.0.0.1:"),EditText($"{ConfigData.Port}/{ConfigData.UniversalPath}") ,//                                       NewLine,BgText("        reverse_api_url: ws://127.0.0.1:"),EditText($"{ConfigData.Port}/{ConfigData.ApiPath}"),//                                       NewLine,BgText("        reverse_event_url: ws://127.0.0.1:"),EditText($"{ConfigData.Port}/{ConfigData.EventPath}"),
                                       NewLine,BgText("        reverse_reconnect_interval: 3000"),
                                       NewLine,BgText("    }"),
                                       NewLine,BgText("]"),
                                       NewLine,SeptTxt,
                                       NewLine,BgText("post_message_format: "),EditText("array"),
                                       NewLine,SeptTxt
                                   }
                                } ,
                               new Cell() ,
                               new  Border()
                               {
                                   TextAlign   =TextAlign.Left,
                                   Background=ConsoleColor.Blue,
                                   Align=Align.Stretch,
                                   Margin=new Thickness(1,0,0,0),
                                   Children = {
                                       new  Border(new Span("github.com/yyuueexxiinngg/onebot-kotlin"){Color=ConsoleColor.DarkBlue,Background=ConsoleColor.DarkCyan}){Align=Align.Center},
                                       new Separator(),
                                       SeptTxt,
                                       NewLine,BgText("    heartbeat:"),
                                       NewLine,BgText("      enable: "),EditText("true"),
                                       NewLine,SeptTxt,
                                       NewLine,BgText("    ws_reverse: "),
                                       NewLine,BgText("      - enable: "),EditText("true") ,
                                       NewLine,BgText("        postMessageFormat: "),EditText("array") ,
                                       NewLine,BgText("        reverseHost: 127.0.0.1"),
                                       NewLine,BgText("        reversePort: "),EditText(ConfigData.Port.ToString()),
                                       NewLine,BgText("        accessToken: ''"),
                                       NewLine,BgText("        reversePath: "),EditText(string.IsNullOrEmpty(ConfigData.UniversalPath)?"''":ConfigData.UniversalPath),                                       //NewLine,BgText("        reverseApiPath: "),EditText(string.IsNullOrEmpty(ConfigData.ApiPath)?"''":ConfigData.ApiPath),                                       //NewLine,BgText("        reverseEventPath: "),EditText(string.IsNullOrEmpty(ConfigData.EventPath)?"''":ConfigData.EventPath),
                                       NewLine,SeptTxt
                                   }
                                }
                                      }
                                  }
                               );
                ConsoleRenderer.RenderDocument(doc);
            }
            catch (Exception) { }
            _ = Task.Run(() =>
            {
                while (true)
                {
                    var read = Console.ReadLine();
                    switch (read.Trim().ToLower())
                    {
                        case "help":
                            Console.WriteLine("未完成");
                            break;
                        case "stop":
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("未知命令:" + read);
                            break;
                    }
                }
            });
            await service.StartService().RunCatch(e => WriteLineErr("[Sora Service]" + e.ToString()));
            await Task.Delay(-1);
        }
    }
}
