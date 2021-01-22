Imports WebSocketSharp
Imports PFBridgeCore.PFWebsocketAPI.Model
Namespace Ws
    Public Class Connection
        Implements IBridgeMCBase
        Public Sub RunCmd(cmd As String) Implements IBridgeMCBase.RunCmd
            Try
                Dim packet1 As ActionRunCmd = New ActionRunCmd(cmd, "", Nothing)
                Dim packet2 = New EncryptedPack(EncryptionMode.AES256, packet1.ToString(), Token)
                Client.Send(packet2.ToString)
            Catch ex As Exception
                API.LogErr(ex)
            End Try


        End Sub
        Public Sub RunCmd(cmd As String, callback As Action(Of String)) Implements IBridgeMCBase.RunCmdCallback
            Dim packet1 = New ActionRunCmd(cmd, Rnd() * Integer.MaxValue, Nothing)
            CmdQueue.Add(New WaitingModel(packet1, callback))
            Dim packet2 = New EncryptedPack(EncryptionMode.AES256, packet1.ToString(), Token)
            Client.Send(packet2.ToString)
        End Sub
        Public Sub New(url As String, _token As String, _tag As Object)
            Id = IdAll : IdAll += 1
            Tag = _tag
            Client = New WebSocket(url)
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
            Client.ConnectAsync()
            CheckTimer.Start()
        End Sub
        Public Property Client As WebSocket
        Public Property Id As Integer Implements IBridgeMCBase.Id
        Private Shared IdAll As Integer = 0
        Public Property Token As String

        Public Property Tag As Object Implements IBridgeMCBase.Tag

        Private CheckTimer As New Timers.Timer(10000) With {.AutoReset = True}
        Private Sub CheckConnect()
            If Not Client.IsAlive Then
                Client.ConnectAsync()
            End If
        End Sub
    End Class
End Namespace
