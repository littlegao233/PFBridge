'#If NETFULL Then
Imports WebSocketSharp
'#Else
'Imports sta_websocket_sharp_core
'#End If

Imports PFBridgeCore.PFWebsocketAPI.Model

Namespace Ws
    Public Class Connection
        Implements IBridgeMCBase
        Public Sub RunCmd(cmd As String) Implements IBridgeMCBase.RunCmd
            Try
                Dim packet1 As ActionRunCmd = New ActionRunCmd(cmd, "", Nothing)
                Dim packet2 = New EncryptedPack(EncryptionMode, packet1.ToString(), Token)
                If Client.IsAlive Then
                    Client.SendAsync(packet2.ToString, Sub(result)
                                                       End Sub)
                Else
                    CheckConnect()
                    If Not CheckTimer.Enabled Then CheckTimer.Start()
                End If
            Catch ex As Exception
                API.LogErr(ex)
            End Try
        End Sub
        Public Sub RunCmd(cmd As String, callback As Action(Of String)) Implements IBridgeMCBase.RunCmdCallback
            Dim packet1 = New ActionRunCmd(cmd, Guid.NewGuid().ToString(), Nothing)
            CmdQueue.Add(New WaitingModel(packet1, callback))
            Dim packet2 = New EncryptedPack(EncryptionMode, packet1.ToString(), Token)
            If Client.IsAlive Then
                Client.SendAsync(packet2.ToString, Sub(result)
                                                   End Sub)
            Else
                CheckConnect()
                If Not CheckTimer.Enabled Then CheckTimer.Start()
            End If
        End Sub
        Public Sub New(url As String, _token As String, _tag As Object)
            Id = IdAll : IdAll += 1
            Tag = _tag
            Token = _token
            Client = New WebSocket(url)
            Client.Log.Output = Sub(data, e)
                                    Select Case data.Level
                                        Case LogLevel.Trace, LogLevel.Debug, LogLevel.Info
                                            API.Log($"{Client.Url}[ws|{data.Level}]{data.Message}")
                                        Case LogLevel.Fatal
                                            'API.LogErr()
                                            If data.Caller.GetMethod().Name = "<startReceiving>b__2" Then Return
                                            If data.Message.ToLower.StartsWith("no connection could be made because the target machine actively refused it.") Then API.LogErr($"{Client.Url}[ws|{data.Level}]无法建立连接，因为目标计算机主动拒绝了该连接") : Return
                                            API.LogErr($"{Client.Url}[ws|{data.Level}]{data.Message}")
                                        Case LogLevel.Error, LogLevel.Warn
                                            API.LogErr($"{Client.Url}[ws|{data.Level}]{data.Message}")
                                    End Select
                                End Sub
            AddHandler Client.OnMessage, Sub(sender, e)
                                             ProcessMessage(Me, e.Data)
                                         End Sub
            'AddHandler Client.OnOpen, Sub(sender, e)
            '                              API.Log($"与{Client.Url}的连接已建立 ")
            '                          End Sub
            'AddHandler Client.OnError, Sub(sender, e)
            '                               API.LogErr($"{Client.Url}遇到错误：{ e.Exception}")
            '                           End Sub
            AddHandler Client.OnClose, Sub(sender, e)
                                           If e.Code = 1006 Then
                                               API.LogErr($"{Client.Url}建立连接失败，将自动尝试重连[" & e.Code & "]:尝试连接时发生异常“)
                                           Else
                                               API.LogErr($"{Client.Url}断开连接，将自动尝试重连[" & e.Code & "]:" & e.Reason)
                                           End If
                                       End Sub
            AddHandler CheckTimer.Elapsed, Sub(sender, e)
                                               Try
                                                   CheckConnect()
                                               Catch : End Try
                                           End Sub
        End Sub
        Public Property Client As WebSocket
        Public Property Id As Integer Implements IBridgeMCBase.Id
        Private Shared IdAll As Integer = 0
        Public Property Token As String

        Public Property Tag As Object Implements IBridgeMCBase.Tag

        Private ReadOnly CheckTimer As New Timers.Timer() With {.AutoReset = True, .Interval = 5000, .Enabled = True}
        Private Sub CheckConnect() Implements IBridgeMCBase.CheckConnect
            Try
                If Not Client.IsAlive Then
#If DEBUG Then
                    API.Log(Client.ReadyState)
#End If
                    Client.ConnectAsync()
                End If
            Catch ex As InvalidOperationException
            Catch ex As Exception
#If DEBUG Then
                API.LogErr(ex.ToString)
#End If
            End Try
        End Sub
        Public Shared Property EncryptionMode As EncryptionMode = EncryptionMode.aes_cbc_pck7padding
    End Class
End Namespace
