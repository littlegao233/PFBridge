using SDK.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFBridgeForXiaoLz.Plugin
{
    public class OpenRobotMenu : IAppSetting
    {
        public void AppSetting()
        {
            System.Windows.Forms.MessageBox.Show("无设置窗体\n插件详细状态见控制台(运行日志)","暂无窗体");
        }
    }
}
