﻿Imports System
Imports HuajiTech.CoolQ
Imports HuajiTech.CoolQ.Events
Imports PFBridgeCore.APIs.EventsMap.QQEventsMap
Namespace PFBridgeForCQ
    ''' <summary>
    ''' 包含应用的逻辑。
    ''' </summary>
    Friend Class Main
        Inherits Plugin
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
            PFBridgeCore.Events.QQ.OnGroupMessage.Invoke(New GroupMessageEventsArgs(e.Source.Number, e.Sender.Number, e.Message.Content))
        End Sub
    End Class
End Namespace
