Imports System
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
            'e.Sender.Nickname

            PFBridgeCore.Events.QQ.OnGroupMessage.Invoke(New GroupMessageEventsArgs(
                                                                                    e.Source.Number,
                                                                                    e.Sender.Number,
                                                                                    CQDeCode(e.Message.Content),
                                                                                    Function() e.Source.DisplayName,
                                                                                    Function() e.Sender.Nickname,
                                                                                    Function() e.Sender.DisplayName))
        End Sub
        Public Shared Function CQDeCode(source As String) As String
            If (source Is Nothing) Then Return String.Empty
            Dim builder As New Text.StringBuilder(source)
            builder = builder.Replace("&#91;", "[")
            builder = builder.Replace("&#93;", "]")
            builder = builder.Replace("&#44;", ",")
            builder = builder.Replace("&amp;", "&")
            Return builder.ToString()
        End Function
    End Class

End Namespace
