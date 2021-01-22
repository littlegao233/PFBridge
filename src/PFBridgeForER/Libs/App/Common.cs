using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity;

namespace IBoxs.Core.App
{
	/// <summary>
	/// 用于存放 App 数据的公共类
	/// </summary>
	public static class Common
    {
       
        /// <summary>
        /// 获取或设置当前 App 使用的依赖注入容器实例
        /// </summary>
        public static IUnityContainer UnityContainer { get; set; }

    }
}
