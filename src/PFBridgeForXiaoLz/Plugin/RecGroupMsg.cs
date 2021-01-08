using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using SDK;
using SDK.Events;
using SDK.Interface;
using SDK.Model;

namespace PFBridgeForXiaoLz.Plugin
{
    public class RecGroupMsg : IGroupMessage
    {
        public void ReceviceGroupMsg(GroupMessageEvent e)
        {
            if (e.SenderQQ == 1000032 || e.ThisQQ == e.SenderQQ)//不处理匿名信息和自己
            {
                return;
            }
            // Common.xlzAPI.SendGroupMessage(e.ThisQQ, e.MessageGroupQQ, "测试小栗子C# SDK", true);
            // Common.xlzAPI.RecviceImage(e.MessageContent, e.ThisQQ, e.SenderQQ);
            //if (e.MessageContent.Contains("转发"))
            //{
            //    Common.xlzAPI.SendGroupMessage(e.ThisQQ, e.MessageGroupQQ, e.MessageContent);
            //}
            //if (e.MessageContent.Equals("群签到"))
            //{
            //    Common.xlzAPI.GroupSigninEvent(e.ThisQQ, e.MessageGroupQQ);
            //}
            //if (e.MessageContent.Equals("领红包"))
            //{
            //    Common.xlzAPI.GetReceiveRedEnvelopeEvent(e.ThisQQ, e.MessageGroupQQ,e.SenderQQ, "", 2,"");
            //}
            App.OnMessageReceived(ref e);
        }
     }
}
