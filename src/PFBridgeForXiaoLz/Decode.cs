using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFBridgeCore;
namespace PFBridgeForXiaoLz
{
    internal static class Decode
    {
        internal static string ParseMessage(string msg, long group, long thisQQ)
        {
            StringBuilder builder = new StringBuilder(msg);
            //获取字符串中所有XiaoLzTextCode
            var got = SDK.Core.TextFormat.XiaoLzTextCode.Parse(msg);
            //foreach (var x in got)//遍历获取到的每个XiaoLzTextCode
            //{
            //    var f = x.Function;//获取到的类型
            //    if (f == SDK.Core.TextFormat.XiaoLzFunction.At)
            //    { var 艾特的QQ号 = x.Target; }
            //    if (f == SDK.Core.TextFormat.XiaoLzFunction.bigFace)
            //    { var 表情的名字 = x.Items["name"]; }
            //}
            System.IO.File.WriteAllText("test.json", Newtonsoft.Json.JsonConvert.SerializeObject(got));
            foreach (var item in got)
            {
                switch (item.Function)
                {
                    case SDK.Core.TextFormat.XiaoLzFunction.At:
                        string cd = SDK.Common.xlzAPI.GetOneGroupMemberInfo(thisQQ, group, item.Target).GroupCardName;
                        builder.Replace(item.Original, string.Format(APIs.API.ParseMessageFormat.At, cd));
                        break;
                    case SDK.Core.TextFormat.XiaoLzFunction.AtAll:
                        builder.Replace(item.Original, string.Format(APIs.API.ParseMessageFormat.AtAll));
                        break;
                    case SDK.Core.TextFormat.XiaoLzFunction.Face:
                    case SDK.Core.TextFormat.XiaoLzFunction.bigFace:
                    case SDK.Core.TextFormat.XiaoLzFunction.smallFace:
                    case SDK.Core.TextFormat.XiaoLzFunction.bq:
                        if (item.Items.TryGetValue("name", out string name))
                            builder.Replace(item.Original, string.Format(APIs.API.ParseMessageFormat.Bface, name));
                        else
                            builder.Replace(item.Original, APIs.API.ParseMessageFormat.Face);
                        break;
                    //case SDK.Core.TextFormat.XiaoLzFunction.Shake:
                    //    break;
                    //case SDK.Core.TextFormat.XiaoLzFunction.limiShow:
                    //    break;
                    //    break;
                    //case SDK.Core.TextFormat.XiaoLzFunction.flashWord:
                    //    break;
                    //case SDK.Core.TextFormat.XiaoLzFunction.Honest:
                    //    break;
                    case SDK.Core.TextFormat.XiaoLzFunction.flashPicFile:
                    case SDK.Core.TextFormat.XiaoLzFunction.Graffiti:
                    case SDK.Core.TextFormat.XiaoLzFunction.picShow:
                    case SDK.Core.TextFormat.XiaoLzFunction.picFile:
                    case SDK.Core.TextFormat.XiaoLzFunction.pic:
                        builder.Replace(item.Original, APIs.API.ParseMessageFormat.Image);
                        break;
                    case SDK.Core.TextFormat.XiaoLzFunction.file:
                        if (item.Items.TryGetValue("fileName", out string filename))
                            builder.Replace(item.Original, string.Format(APIs.API.ParseMessageFormat.File, filename));
                        else builder.Replace(item.Original, string.Format(APIs.API.ParseMessageFormat.File, "未知文件"));
                        break;
                    //    break;
                    case SDK.Core.TextFormat.XiaoLzFunction.litleVideo:
                        builder.Replace(item.Original, APIs.API.ParseMessageFormat.Video);
                        break;
                    case SDK.Core.TextFormat.XiaoLzFunction.AudioFile:
                        builder.Replace(item.Original,APIs.API.ParseMessageFormat.Record);
                        break;
                    //case SDK.Core.TextFormat.XiaoLzFunction.Sticker:
                    //    //builder.Replace(item.Original, string.Format(APIs.API.ParseMessageFormat.Record));
                    //    break;
                    case SDK.Core.TextFormat.XiaoLzFunction.Share:
                        builder.Replace(item.Original, string.Format(APIs.API.ParseMessageFormat.Share));
                        break;
                    //case SDK.Core.TextFormat.XiaoLzFunction.Other:
                    //    builder.Replace(item.Original, string.Format(APIs.API.ParseMessageFormat.Share));
                    //    break;
                    default:
                        builder.Replace(item.Original, string.Format(APIs.API.ParseMessageFormat.Unknown,item.FunctionTypeRaw));
                        break;
                }
            }
            return SDK.Core.TextFormat.TextCodeDecode(builder.ToString());
            //计分板完成
        }
    }
}
