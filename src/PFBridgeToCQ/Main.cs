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
        public Main(IMessageEventSource messageEventSource)
        {
            try
            {
                PFBridgeCore.Main.Init(new API());//主体注入
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
            messageEventSource.AddMessageReceivedEventHandler(OnMessageReceived);
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