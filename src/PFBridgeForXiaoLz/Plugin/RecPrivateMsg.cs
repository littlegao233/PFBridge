﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SDK;
using SDK.Events;
using SDK.Interface;

namespace PFBridgeForXiaoLz.Plugin
{
    public class RecPrivateMsg : IRecvicetPrivateMessage
    {
        public void RecvicetPrivateMsg(PrivateMessageEvent e)
        {
            //if (e.ThisQQ == e.SenderQQ)
            //{
            //    return;
            //}
            //Common.xlzAPI.OutLog("我输出的日志");
            //Common.xlzAPI.SendPrivateMessage(e.ThisQQ, e.SenderQQ, "欢迎使用小栗子SDK");
            //Common.xlzAPI.RecviceImage(e.MessageContent, e.ThisQQ, e.SenderQQ);
            //Common.xlzAPI.GetFriendList(e.ThisQQ);
            //List<SDK.Model.GroupInfo> groupInfos = Common.xlzAPI.Getgrouplist(e.ThisQQ);
            //if (groupInfos != null)
            //{
            //    Common.xlzAPI.GetgroupMemberlist(e.ThisQQ, groupInfos[0].GroupID);
            //}
            //Common.xlzAPI.GetAdministratorList(e.ThisQQ,535107725);
            //Common.xlzAPI.GetgroupMemberlist(e.ThisQQ, 535107725);
            //Common.xlzAPI.GetFriendInfoEvent(e.ThisQQ, 414725048);
            //Common.xlzAPI.CreateGroupFolderEvent(e.ThisQQ, 247681297, "小栗子");
            //Common.xlzAPI.SendFreeGiftEvent(e.ThisQQ, 247681297, 414725048, SDK.Enum.FreeGiftEnum.Gift_280);
            //Common.xlzAPI.GetGroupFileListEvent(e.ThisQQ, 480325208,"");
            //string imagepath = System.Environment.CurrentDirectory + "\\上传好友图片.png";
            //string piccode = Common.xlzAPI.UploadGroupImage(e.ThisQQ, 480325208, imagepath, false);
            //if (e.MessageContent.Contains("转发"))
            //{
            //    Common.xlzAPI.SendPrivateMessage(e.ThisQQ, e.MessageGroupQQ, e.MessageContent);
            //}
            //for (int i = 0; i < 600; i++)
            //{

            //    //SDK.Core.API aPI = new SDK.Core.API();
            //    //aPI.SendPrivateMessage(e.ThisQQ, e.SenderQQ, "VDXXH-QDN62-K2MTB-G2P88-Q7B86\r\n无法安装此密钥\r\n0xC004F025 \r\n2020/09/05 17:57:29（UTC+8)");
            //    //aPI.SendPrivateMessage(e.ThisQQ, e.SenderQQ, i.ToString());
            //    //System.Threading.Thread.Sleep(200);
            //    Common.xlzAPI.SendPrivateMessage(e.ThisQQ, e.SenderQQ, "VDXXH-QDN62-K2MTB-G2P88-Q7B86\r\n无法安装此密钥\r\n0xC004F025 \r\n2020/09/05 17:57:29（UTC+8)");
            //    Common.xlzAPI.SendPrivateMessage(e.ThisQQ, e.SenderQQ, i.ToString());
            //    System.Threading.Thread.Sleep(200);
            //}
            //Common.xlzAPI.GetGroupMemberBriefInfoEvent(e.ThisQQ, 480325208);247681297
            //string cookies = Common.xlzAPI.GetWebCookiesEvent(e.ThisQQ, "https://h5.qzone.qq.com/mqzone/index", "549000929", "5");
            string picpath = System.Environment.CurrentDirectory + "\\图片.png";
            if (e.MessageContent.Equals("发送群公告"))
            {
                if (File.Exists(picpath))
                {
                    string ret = Common.xlzAPI.SetAnnouncementEvent(e.ThisQQ, 247681297, "小栗子发公告", "测试发公告", picpath, null,true, true, true, true, true);
                }
            }
            //if (e.MessageContent.Contains("[Shake,name="))
            //{
            //    string Shakepath = System.Environment.CurrentDirectory + "\\Shake.txt";
            //    using (FileStream fs = new FileStream(Shakepath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            //    {
            //        fs.Position = fs.Length;
            //        using (StreamWriter sw = new StreamWriter(fs))
            //        {
            //            sw.WriteLine(e.MessageContent);
            //            sw.Close();
            //        }
            //        fs.Close();
            //    }
            //}
            if (e.MessageContent.Equals("取钱包"))
            {
                Common.xlzAPI.GetQQWalletPersonalInformationEvent(e.ThisQQ);
            }
        }
        
    }
}
