moduleInfo.Author = "littlegao233";
moduleInfo.Version = "v0.0.1";
moduleInfo.Description = '此JS定义了消息特殊内容的替换方式';
var api = importNamespace('PFBridgeCore').APIs.API;
var format = api.ParseMessageFormat;
var defaultColor = "§r§a";
format.At = "§r§l§6@§r§6{0}" + defaultColor;
format.AtAll = "§r§l§g@§r§g全体成员" + defaultColor;
format.Image = "§r§l§d[图骗]" + defaultColor;
format.Emoji = "§r§l§d[emoji]" + defaultColor;
format.Face = "§r§l§c[表情]" + defaultColor;
format.Bface = "§r§l§d[大表情:§r§o§7{0}§r§l§d]" + defaultColor;
format.Record = "§r§l§g[语音]" + defaultColor;
format.Video = "§r§l§b[视频]" + defaultColor;
format.Share = "§r§l§b[分享§r§e:{1}§d({0})§l§b]" + defaultColor;
format.Music = "§r§l§d[音乐§r§d:{1}§l§b]" + defaultColor;
format.Reply = "§r§l§7[回复]" + defaultColor;
format.Forward = "§r§l§7[转发]" + defaultColor;
format.Node = "§r§l§7[转发节点]" + defaultColor;
format.Xml = "§r§l§7[富文本消息]" + defaultColor;
format.Json = "§r§l§7[富文本消息]" + defaultColor;
format.File = "§r§l§b[文件:§r§o§7{0}§r§l§d]" + defaultColor;
format.Unknown = "§r§l§7[{0}]" + defaultColor;
