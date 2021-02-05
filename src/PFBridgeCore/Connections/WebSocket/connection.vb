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
            Client = New WebSocket(url) 'New Microsoft.Extensions.Logging.ILogger

            Token = _token
            AddHandler Client.OnMessage, Sub(sender, e)
                                             ProcessMessage(Me, e.Data)
                                         End Sub
            AddHandler Client.OnOpen, Sub(sender, e)
                                          API.Log($"与{Client.Url}的连接已建立 ")
                                      End Sub
            AddHandler Client.OnError, Sub(sender, e)
                                           API.LogErr($"{Client.Url}遇到错误：{ e.Exception}")
                                       End Sub
            AddHandler Client.OnClose, Sub(sender, e)
                                           API.LogErr($"{Client.Url}断开连接，将自动尝试重连： " & e.Reason)
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
                    Client.ConnectAsync()
                End If
            Catch ex As Exception
#If DEBUG Then
                API.LogErr(ex.ToString)
#End If
            End Try
        End Sub
        Public Shared Property EncryptionMode As EncryptionMode = EncryptionMode.aes_cbc_pck7padding
    End Class
End Namespace
