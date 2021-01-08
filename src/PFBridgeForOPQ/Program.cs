using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Traceless.OPQSDK.Models;
using Traceless.OPQSDK.Models.Event;
using Traceless.OPQSDK.Models.Msg;
using Traceless.SocketIO;
using Traceless.SocketIO.Messages;
using Traceless.Utils.Ai.Tencent;

namespace Traceless.Robot
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            while (true)
            {
                try
                {
                    await OPQSDK.Plugin.OPQMain.Client();
                    while (true) { Console.ReadLine(); }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("崩溃啦！(" + ex.ToString() + ")");
                    Console.WriteLine("将在5s后重启.");
                    System.Threading.Thread.Sleep(5000);
                }
            }
        }
    }
}