Imports System
Imports HuajiTech.CoolQ
Imports HuajiTech.CoolQ.Events

Namespace PFBridgeForCQ
    ''' <summary>
    ''' 提供对应用的初始化。
    ''' </summary>
    Friend Module App
        ''' <summary>
        ''' 初始化应用。
        ''' </summary>
        ''' <remarks>
        ''' 该方法会在酷Q主线程内被调用，不允许调用酷Q API，且不应长时间阻塞线程。
        ''' 在该方法内引发的异常将会导致酷Q主程序停止运行。
        ''' </remarks>
        Public Sub Init()
            Console.WriteLine("[PFBridgeForCQ] Framework Start Loading ...")
            currentUserEventSource = Events.CurrentUserEventSource.Instance
            groupEventSource = Events.GroupEventSource.Instance
            Dim botEventSource As IBotEventSource = Events.BotEventSource.Instance
            AddHandler botEventSource.AppDisabling, AddressOf BotEventSource_AppDisabling
            AddHandler botEventSource.AppEnabled, AddressOf BotEventSource_AppEnabled
            ' 使用下面的代码在酷Q初始化后创建 Main 类的实例。
            ' 需要在 app.json 中注册对应事件。 
        End Sub

        Friend currentUserEventSource As ICurrentUserEventSource
        Friend groupEventSource As IGroupEventSource
        Private runner As Main

        Private Sub BotEventSource_AppEnabled(ByVal sender As Object, ByVal e As EventArgs)
            CurrentPluginContext.Logger.LogDebug("BotEventSource_AppEnabled 事件开始处理...")
            runner?.Dispose()
            runner = New Main() '注入主体部分
        End Sub

        Private Sub BotEventSource_AppDisabling(ByVal sender As Object, ByVal e As EventArgs)
            CurrentPluginContext.Logger.LogDebug("BotEventSource_AppDisabling 事件开始处理...")
            runner?.Dispose()
        End Sub
    End Module
End Namespace
