using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDK.Events;
using SDK.Interface;

namespace PFBridgeForXiaoLz.Plugin
{
    public class AppEnable : IAppEnableEvent
    {
        public void AppEnableEvent(AppEnableEvent e)
        {
            App.OnStartup();
        }
    }
}
