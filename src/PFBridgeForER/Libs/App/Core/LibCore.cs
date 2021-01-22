using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using System.Runtime.InteropServices;
using System.Windows.Forms;
//using IBoxs.Sdk.Cqp.EventArgs;
using System.IO;

namespace IBoxs.Core.App.Core
{
   public class OnoQQ_Fun
    {
        private static Encoding _defaultEncoding = null;
       
        /// <summary>
        /// 静态构造函数, 注册依赖注入回调
        /// </summary>
        static OnoQQ_Fun()
        {
            _defaultEncoding = Encoding.GetEncoding("GB18030");

            // 初始化 Costura.Fody
            CosturaUtility.Initialize();

            // 初始化依赖注入容器
            Common.UnityContainer = new UnityContainer();
            
        }
         
}
}
