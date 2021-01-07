Imports QQMini.PluginSDK.Core
Imports QQMini.PluginSDK.Core.Model

Public Class Class1
    Inherits PluginBase
    Public Overrides ReadOnly Property PluginInfo As PluginInfo
        Get
            Dim info As PluginInfo = New PluginInfo With {
                .PackageId = "com.jiegg.demo",
                .Name = "复读机",
                .Version = New Version(1, 0, 0, 0),
                .Author = "JieGG",
                .Description = "QQMini插件教程插件 (V3 插件机制)"
            }
            Return info
        End Get
    End Property
    Public Overrides Function OnReceiveGroupMessage(e As QMGroupMessageEventArgs) As QMEventHandlerTypes
        QMApi.SendGroupMessage(e.RobotQQ, e.FromGroup, e.Message)
        Return QMEventHandlerTypes.Continue ' 返回继续时, 后续的插件将会接收到此消息
    End Function
End Class
