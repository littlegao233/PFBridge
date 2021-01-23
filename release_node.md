> #### 这里是最近一次成功的[AzurePipeline构建存档](https://dev.azure.com/gaoxinhong2004/PFBridge/_build?definitionId=2&view=runs&statusFilter=succeeded),由AzurePipeline自动从最新的GitHub提交编译生成
### 版本说明
 - <h4>PFBridgeForCQ.zip内含dll和json，用于原酷Q加载器(如Mirai Native和CQXQ)</h4>
 - #### PFBridge.OQ.dll用于OnoQQ框架
 - #### PFBridge.XiaoLz.dll用于小栗子框架
 - #### PFBridge.XQ.dll用于先驱机器人框架<h6>（注意先驱需要加载后重启框架，即保证启动框架的时候已加载，否则有bug）</h6>
 - #### PFBridge.IR.dll用于支持IRQQ插件的加载器
 - - ##### 重命名为`PFBridge.ER.dll`即可在ERbot框架载入
 - - ##### 重命名为`PFBridge.NT.dll`即可在NutQQ框架载入
 - - ##### 重命名为`PFBridge.OQ.dll`即可在OnoQQ框架载入
 - - ###### （注意需要加载后重启框架，启动后需要手动登录至少1个QQ，插件才能加载）
 - #### PFBridgeForOPQ.zip内含可执行文件，OPQ框架使用（支持linux：[参考信息](https://github.com/traceless0929/Traceless.OPQ)）