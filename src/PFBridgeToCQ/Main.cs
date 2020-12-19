using System;
using System.Collections.Generic;
using System.Text;
using HuajiTech.CoolQ;
using HuajiTech.CoolQ.Events;
using HuajiTech.CoolQ.Messaging;

namespace PFBridgeToCQ
{
    /// <summary>
    /// 包含应用的逻辑。
    /// </summary>
    internal class Main : Plugin
    {
        /// <summary>
        /// 使用指定的事件源初始化一个 <see cref="Main"/> 类的新实例。
        /// </summary>
        public Main()
        {
            try { PFBridgeCore.Main.Init(new API());/*主体注入*/} catch (Exception ex) { Console.WriteLine("主入口注入失败：" + ex); }
            //添加监听
            App.currentUserEventSource.AddMessageReceivedEventHandler(OnMessageReceived);
           
        }
        ~Main() { Dispose(); }
        /// <summary>
        /// 销毁实例
        /// </summary>
        internal void Dispose()
        {
            //删除监听
            App.currentUserEventSource.RemoveMessageReceivedEventHandler(OnMessageReceived);
        }
        /// <summary>
        /// 处理消息接收事件。
        /// </summary>
        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            // 回复一条消息，内容为收到的消息的内容。
            e.Reply(e.Message);
        }
    }
}