<table border="30" align="center">
    <tr>
        <td>
            <div align="center">
                <h1>PFBridge</h1>
                    <h4>Attach QQGroup to MCBE server</h4>
            </div>
            <div align="center">
                <blockquote align="center">

[![](https://img.shields.io/badge/Minebbs-%E8%AE%BA%E5%9D%9B-ea2bc7?style=flat-square)](https://www.minebbs.com/resources/1975/)
[![](https://img.shields.io/github/downloads/littlegao233/PFBridge/total?color=ea2bc7&label=%E4%B8%8B%E8%BD%BD&style=flat-square)](https://github.com/littlegao233/PFBridge/releases)
[![](https://img.shields.io/github/v/release/littlegao233/PFBridge?color=ea2bc7&label=%E6%9C%80%E6%96%B0%E5%8F%91%E5%B8%83&style=flat-square)](https://github.com/littlegao233/PFBridge/releases/latest)
[![](https://img.shields.io/github/license/littlegao233/PFBridge?color=ea2bc7&label=%E5%BC%80%E6%BA%90%E5%8D%8F%E8%AE%AE&style=flat-square)](https://github.com/littlegao233/PFBridge/blob/main/LICENSE)
![GitHub Repo stars](https://img.shields.io/github/stars/littlegao233/PFBridge?color=ea2bc7&label=%E2%98%85&style=flat-square)
                </blockquote> 
                <blockquote align="center">

[![](https://img.shields.io/azure-devops/build/gaoxinhong2004/88fb96c4-21cf-4d12-91ec-54a1c36c21b7/2?color=ea2bc7&label=%E6%9E%84%E5%BB%BA&style=flat-square)](https://dev.azure.com/gaoxinhong2004/PFBridge/_build?definitionId=2)
[![](https://img.shields.io/azure-devops/release/gaoxinhong2004/88fb96c4-21cf-4d12-91ec-54a1c36c21b7/2/2?color=ea2bc7&label=%E9%83%A8%E7%BD%B2&style=flat-square)](https://dev.azure.com/gaoxinhong2004/PFBridge/_release?view=all&_a=releases&definitionId=2)
![](https://img.shields.io/github/languages/count/littlegao233/PFBridge?color=ea2bc7&style=flat-square)
![](https://img.shields.io/github/languages/top/littlegao233/PFBridge?color=ea2bc7&style=flat-square)
![](https://img.shields.io/github/last-commit/littlegao233/PFBridge?color=ea2bc7&label=%E6%9C%80%E5%90%8E%E6%8F%90%E4%BA%A4&style=flat-square)
![](https://img.shields.io/github/commit-activity/m/littlegao233/PFBridge?color=ea2bc7&label=%E6%8F%90%E4%BA%A4%E9%A2%91%E7%8E%87&style=flat-square)
                </blockquote>
                <div align="center">
                



</div>
            </div>
        </td>
    </tr>
</table>
<hr>


                

- ## State/开发状态
   - 基础脚本,支持简单的[自定义配置](#QQ部分)
   - （目前更新可能需要删除部分旧脚本文件）
---
- ## Features/特征
   - 多框架支持
   - JavaScript自定义配置
---
- ## [Progress/进度](https://github.com/littlegao233/PFBridge/projects/1)
    - [x] WebSocket协议对接服务器方案
    - - [x] 兼容[LiteLoaderWebSocketR - by WangYneos](https://www.minebbs.com/resources/2150)`推荐`(最新版本已适配，测试成功)
    - - [x] 兼容[PFWebsocketAPI - Minebbs](https://www.minebbs.com/resources/1632/)(基于CSR)
    - - [x] 兼容`BDXWebSocketR - by WangYneos`BDX群内发布(最新版本已适配，尚未测试)
    - - [ ] `ElementZeroR`的webapi(还没出)
    - [x] 可拓展基础架构
    - - [x] JavaScript引擎实现自定义功能
    - [x] 多机器人框架支持
    - - [x] 原酷Q插件支持（兼容Mirai Native、CQXQ等）
    - - [x] 先驱机器人框架支持
    - - [X] OnoQQ机器人框架支持
    - - [X] 小栗子框架支持
    - - [x] [OPQ](https://github.com/OPQBOT/OPQ)客户端支持
    - - [x] go-cqhttp客户端支持
    - - [X] ERbot机器人框架支持(重命名`PFBridge.IR.dll`为`PFBridge.ER.dll`即可载入)
    - - [x] NutQQ支持(重命名`PFBridge.IR.dll`为`PFBridge.NT.dll`即可载入)
    - - [ ] Telegram支持(编写中)
    - - [ ] Nanbot支持（咕咕咕，太多了没人用
    - - [ ] QQmini框架支持(咕咕咕，太多了没人用
    - - [ ] 可爱猫微信机器人框架支持
    - - [ ] Discord支持
    - - [ ] 钉钉支持
---
- ## Usage/使用方法
>---
>- ### MC服务器部分
>- - 下载PFWebsocketAPI([Minebbs](https://www.minebbs.com/>resources/csr-pfwebsocketapi.1632/)|[Github](https://github.>com/littlegao233/PFWebsocketAPI/releases))
>- - 使用BDSNetRunner加载PFWebsocketAPI.csr.dll(不再赘述)
>- - 配置好并记下`[BDS]\plugins\PFWebsocket\config.json`的配置参数
>``` jsonc
>{
>  "Port": "29132",//Websocket连接端口
>  "EndPoint": "mcws",//websocket连接终端（地址最后"/"后面的）
>  "Password": "commandpassword"//交换密码（用于运行命令等操作）
>}
>```
>---
>- ### QQ部分
>- - 启动某机器人框架
>- - 下载对应已构建文件(<a href="https://github.com/littlegao233/PFBridge/releases">最新可执行文件</a>)
>- - 根据提示加载本插件(不再赘述)
>- - 加载完首次后可在控制台提示中看到对应的插件数据目录(如Mirai是`[Mirai]\data\MiraiNative\data\PFBridge.CQ`),打开该目录:
>- - - `index.js`是加载器入口,不建议修改
>- - - `libs`文件夹用于加载net类库(.dll)，加载后可在脚本中使用`const xxx = importNamespace('net命名空间')`来使用
>- - - `scripts`文件夹存放的是主体部分的脚本
>- 使用专业的编辑器(别拿个记事本截图问怎么用)(如[VisualStudioCode](https://code.visualstudio.com/))打开插件数据目录下的`config.json`
>- 可以在文件中看到详细的注释，自行修改对应内容,然后重启框架（重载插件）
>```jsonc
>{
>    "AdminQQs": [441870948, 233333]/*管理员QQ号,用于配置是否可执行命令等*/,
>    "Groups": [
>        {
>            "id": 626872357,//QQ群号
>            "ServerMsgToGroup": true,//是否转发服务器的各种消息到该群
>            "GroupMsgToServer": true//是否将该群的消息转发到所有服务器
>        }
>    ],
>    "Servers": [
>        {
>            "type": "websocket",
>            "url": "ws://127.0.0.1:29132/mcws",//websocket地址|如BDS的{"Port": "29132","EndPoint": "mcws","Password": "commandpassword"}对应ws://127.0.0.1:29132/mcws
>            "token": "commandpassword",//websocket密匙串（用于运行命令等操作）|"Password": "commandpassword"
>            "name": "测试服务器",
>            "ServerMsgToGroup": true,//是否将该服务器的各种消息转发到群
>            "GroupMsgToServer": true,//是否转发群消息到该服务器
>            "ServerMsgToOther": true,//是否将该服务器的各种消息转发到其他已连接服服务器（多服联动）
>            "ReceiveMsgFromOther": true,//是否接受其他服务器的消息（多服联动）
>            "WhitelistEnabled": true//是否开启白名单，改参数主要在whitelist.js中用到
>        }/*, {//在这里添加多个服务器
>            "type": "websocket",
>            "url": "ws://127.0.0.1:29132/mcws",//websocket地址
>            "token": "commandpassword",//websocket密匙串（用于运行命令等操作）
>        }*/
>    ]
>}
>```
---
- ## Thanks/鸣谢
- - (直接或间接使用到的库和源码)
>| name | link |
>|  ----|----  |
>| Jint | https://github.com/sebastienros/jint |
>| Costura.Fody | https://github.com/Fody/Costura |
>| Newtonsoft.Json | https://www.newtonsoft.com/json |
>| DllExport | https://github.com/3F/DllExport |
>| WebSocketSharp | http://sta.github.io/websocket-sharp/ |
>| WebSocketSharp.NetCore | https://github.com/littlegao233/websocket-sharp |
>| SimpleAES | https://github.com/jonjomckay/dotnet-simpleaes |
>| `Sora` | https://github.com/Yukari316/Sora |
>| `CSharpIRQQ` | https://gitee.com/qinghuabumo_admin/CSharpIRQQ |
>| `Traceless.OPQ` | https://github.com/traceless0929/Traceless.OPQ |
>| `ProjHyperai` | https://github.com/theGravityLab/ProjHyperai |
>| `QQMini.PluginSDK` | https://github.com/QQMiniBot/QQMini.PluginSDK |
>| `XQ.Net` | https://gitee.com/heerkaisair/XQ.Net |
>| `HuajiTech.CoolQ` | https://github.com/huajitech/coolq-dotnet-sdk |
>| `XiaolzCSharp` | https://github.com/laomms/XiaolzCSharp |
>| `XiaoLiZi_SDK` | https://github.com/wpyok168/XiaoLiZi_SDK |
>| `IBoxsForCorn` | https://github.com/zqu1016/IBoxsForCorn |
>| `IBoxsForOnoQQ` | https://github.com/zqu1016/IBoxsForOnoQQ |
>| `OneBot 标准` | https://github.com/howmanybots/onebot |



>##### █████████████████
>##### [Telegram交流群](https://t.me/joinchat/TDe2w1ZgMVSw10vbugwg1w) 如有bug可前往反馈
>##### [赞助通道](https://afdian.net/@PF_littlegao233)
>##### █████████████████

