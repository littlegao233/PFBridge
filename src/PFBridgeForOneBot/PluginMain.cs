using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sora.Server;
using static PFBridgeCore.APIs.EventsMap.QQEventsMap;
using PFBridgeCore;

namespace PFBridgeForOneBot
{
    class PluginMain
    {
        private static bool hasStarted = false;
        internal static void AppStart(Sora.Entities.Base.SoraApi api)
        {
            if (hasStarted) return;
            PFBridgeCore.Main.Init(new API(api));
            hasStarted = true;
        }

        internal static void Init(SoraWSServer server)
        {
            server.Event.OnGroupMessage += (sender, eventArgs) =>
          {
              try
              {
                  Sora.EventArgs.SoraEvent.GroupMessageEventArgs e = eventArgs;
                  //e.SoraApi.GetGroupMemberInfo
                  PFBridgeCore.APIs.Events.QQ.OnGroupMessage.Invoke(new GroupMessageEventsArgs(e.SourceGroup.Id, e.Sender.Id, CQDeCode(e.Message.RawText),
                        () =>
                        {
                            var get = e.SourceGroup.GetGroupInfo().AsTask();
                            get.Wait();
                            return get.Result.groupInfo.GroupName;
                        },
                        () => e.SenderInfo.Nick,
                        () => string.IsNullOrEmpty(e.SenderInfo.Card)? e.SenderInfo.Nick : e.SenderInfo.Card,
                        () => (int)e.SenderInfo.Role + 1,
                        //     成员        Member = 0,
                        //     管理员        Admin = 1,
                        //     群主        Owner = 2
                        s => e.Reply(s),
                        () => ParseMessage(e.Message.RawText, e.SourceGroup.Id)
                    ));
              }
              catch (Exception ex) { PFBridgeCore.APIs.API.LogErr(ex); }
              return new ValueTask();
          };
            server.Event.OnClientConnect += (sender, eventArgs) =>
            {
                AppStart(eventArgs.SoraApi);
                return new ValueTask();
            };
            //server.Event.c += async (sender, eventArgs) =>
            //{
            //    await eventArgs.SourceGroup.SendGroupMessage(eventArgs.Message.MessageList);
            //};
        }
        internal static string CQDeCode(string source)
        {
            if (source == null) return string.Empty;
            return CQDeCode(new StringBuilder(source));
        }
        internal static string CQDeCode(StringBuilder builder)
        {
            builder = builder.Replace("&#91;", "[");
            builder = builder.Replace("&#93;", "]");
            builder = builder.Replace("&#44;", ",");
            builder = builder.Replace("&amp;", "&");
            return builder.ToString();
        }
        /// <summary>
		/// 获取字符串副本的转义形式
		/// </summary>
		/// <param name="source">欲转义的原始字符串</param>
		/// <param name="enCodeComma">是否转义逗号, 默认 <code>false</code></param>
		/// <exception cref="ArgumentNullException">参数: source 为 null</exception>
		/// <returns>返回转义后的字符串副本</returns>
		public static string CQEnCode(string source, bool enCodeComma)
        {
            if (source == null) return string.Empty;
            StringBuilder builder = new StringBuilder(source);
            builder = builder.Replace("&", "&amp;");
            builder = builder.Replace("[", "&#91;");
            builder = builder.Replace("]", "&#93;");
            if (enCodeComma)
                builder = builder.Replace(",", "&#44;");
            return builder.ToString();
        }
        internal static string ParseMessage(string raw, long group)
        {
            //StringBuilder builder = new StringBuilder(raw);
            //            foreach (Match m in Regex.Matches(raw, @"
            //\[CQ:                 #最外层的左括号
            //(?<main>
            //    [^\[\]]*           #它后面非括号的内容
            //    (
            //        (
            //        (?'Open'\[)#左括号，压入""Open""
            //        [^\[\]]*      #左括号后面的内容
            //        )+
            //        (
            //        (?'-Open'\]) #右括号，弹出一个""Open""
            //        [^\[\]]*        #右括号后面的内容
            //        )+
            //    )*
            //    (?(Open)(?!))#最外层的右括号前检查
            //                        #若还有未弹出的""Open""
            //                        #则匹配失败
            //)
            //\]                   #最外层的右括号", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture))
            //            {
            StringBuilder builder = new StringBuilder(raw);
            foreach (var code in CQCode.Parse(raw))
            {
                switch (code.Function)
                {
                    case Sora.Enumeration.CQFunction.Face:
                        builder.Replace(code.Original, APIs.API.ParseMessageFormat.Face); break;
                    case Sora.Enumeration.CQFunction.Image:
                        builder.Replace(code.Original, APIs.API.ParseMessageFormat.Image); break;
                    case Sora.Enumeration.CQFunction.Record:
                        builder.Replace(code.Original, APIs.API.ParseMessageFormat.Record); break;
                    case Sora.Enumeration.CQFunction.Video:
                        builder.Replace(code.Original, APIs.API.ParseMessageFormat.Video); break;
                    case Sora.Enumeration.CQFunction.Music:
                        {
                            string url; if (!code.Items.TryGetValue("url", out url)) url = "?";
                            string title; if (!code.Items.TryGetValue("title", out title)) title = "?";
                            builder.Replace(code.Original, string.Format(APIs.API.ParseMessageFormat.Music, url, title)); break;
                        }
                    case Sora.Enumeration.CQFunction.At:
                        string target = code.Items["qq"];
                        if (target == "all") builder.Replace(code.Original, APIs.API.ParseMessageFormat.AtAll);
                        else builder.Replace(code.Original, string.Format(APIs.API.ParseMessageFormat.At, GetMemberCard(group, long.Parse(target))));
                        break;
                    case Sora.Enumeration.CQFunction.Share:
                        {
                            string url; if (!code.Items.TryGetValue("url", out url)) url = "?";
                            string title; if (!code.Items.TryGetValue("title", out title)) title = "?";
                            builder.Replace(code.Original, string.Format(APIs.API.ParseMessageFormat.Share, url, title)); break;
                        }
                    case Sora.Enumeration.CQFunction.Reply:
                        builder.Replace(code.Original, APIs.API.ParseMessageFormat.Reply); break;
                    case Sora.Enumeration.CQFunction.Forward:
                        builder.Replace(code.Original, APIs.API.ParseMessageFormat.Forward); break;
                    //case Sora.Enumeration.CQFunction.Poke:
                    //    builder.Replace(code.Original,APIs.API.ParseMessageFormat.Image); break;
                    case Sora.Enumeration.CQFunction.Xml:
                        builder.Replace(code.Original, APIs.API.ParseMessageFormat.Xml); break;
                    case Sora.Enumeration.CQFunction.Json:
                        builder.Replace(code.Original, APIs.API.ParseMessageFormat.Json); break;
                    //case Sora.Enumeration.CQFunction.RedBag: break;
                    //case Sora.Enumeration.CQFunction.Gift: break;
                    case Sora.Enumeration.CQFunction.CardImage:
                        builder.Replace(code.Original, APIs.API.ParseMessageFormat.Image); break;
                    //case Sora.Enumeration.CQFunction.TTS: break;
                    default:
                        builder.Replace(code.Original, string.Format(APIs.API.ParseMessageFormat.Unknown, code.Function.ToString())); break;
                }
            }
            return CQDeCode(builder);


            //string[] v = m.Groups["main"].Value.Split(',');
            //    switch (v[0])
            //    {
            //        case "face":
            //        case "image": replacement = PFBridgeCore.APIs.API.ParseMessageFormat.Image; break;
            //        case "record": replacement = PFBridgeCore.APIs.API.ParseMessageFormat.Record; break;
            //        case "video": replacement = PFBridgeCore.APIs.API.ParseMessageFormat.Video; break;
            //        case "at":
            //            string target = v[1].Substring(3);
            //            if (target == "all")
            //                replacement = PFBridgeCore.APIs.API.ParseMessageFormat.AtAll;
            //            else
            //                replacement = string.Format(PFBridgeCore.APIs.API.ParseMessageFormat.At, GetMemberCard(group, long.Parse(target)));
            //            break;
            //        case "share": replacement = PFBridgeCore.APIs.API.ParseMessageFormat.Share; break;
            //        case "music": replacement = PFBridgeCore.APIs.API.ParseMessageFormat.Music; break;
            //        case "reply": replacement = PFBridgeCore.APIs.API.ParseMessageFormat.Reply; break;
            //        case "forward": replacement = PFBridgeCore.APIs.API.ParseMessageFormat.Forward; break;
            //        case "emoji": replacement = PFBridgeCore.APIs.API.ParseMessageFormat.Emoji; break;
            //        case "node": replacement = PFBridgeCore.APIs.API.ParseMessageFormat.Node; break;
            //        case "xml": replacement = PFBridgeCore.APIs.API.ParseMessageFormat.Xml; break;
            //        case "json": replacement = PFBridgeCore.APIs.API.ParseMessageFormat.Json; break;
            //        case "Bface": replacement = PFBridgeCore.APIs.API.ParseMessageFormat.Bface; break;
            //    }
            //    builder.Replace(m.Value, replacement);
            //}
            //}
        }
        private static byte mbc = 0;
        internal static string GetMemberCard(long group, long qq)
        {
            bool cache;
            if (mbc > 50) { mbc = 0; cache = false; } else { mbc++; cache = true; }
            var t = API.BaseApi.GetGroupMemberInfo(group, qq, cache).AsTask();
            t.Wait();
            string result = t.Result.memberInfo.Card;
            if (string.IsNullOrEmpty(result )) { result = t.Result.memberInfo.Nick; }
            return result;
        }
    }
}
