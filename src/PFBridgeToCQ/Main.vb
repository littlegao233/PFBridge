Imports System
Imports HuajiTech.CoolQ
Imports HuajiTech.CoolQ.Events
Imports PFBridgeCore.EventArgs

Namespace PFBridgeToCQ
    ''' <summary>
    ''' 包含应用的逻辑。
    ''' </summary>
    Friend Class Main
        Inherits Plugin
        ''' <summary>
        ''' 使用指定的事件源初始化一个 <seecref="Main"/> 类的新实例。
        ''' </summary>
        Public Sub New()
            Try
                PFBridgeCore.Init(New API())
            Catch ex As Exception
                Console.WriteLine("主入口注入失败：" & ex.ToString)
            End Try
            '添加监听
            currentUserEventSource.AddMessageReceivedEventHandler(AddressOf OnMessageReceived)
        End Sub

        Protected Overrides Sub Finalize()
            Dispose()
        End Sub
        ''' <summary>
        ''' 销毁实例
        ''' </summary>
        Friend Sub Dispose()
            '删除监听
            currentUserEventSource.RemoveMessageReceivedEventHandler(New EventHandler(Of MessageReceivedEventArgs)(AddressOf OnMessageReceived))
        End Sub
        ''' <summary>
        ''' 处理消息接收事件。
        ''' </summary>
        Private Sub OnMessageReceived(sender As Object, e As MessageReceivedEventArgs)
            Console.WriteLine("test")
            PFBridgeCore.Events.OnGroupMessage.Invoke(New GroupMessageEventsArgs(e.Source.Number, e.Sender.Number, e.Message.Content))
        End Sub
    End Class
End Namespace
