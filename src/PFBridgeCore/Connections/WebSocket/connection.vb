Imports WebSocketSharp
Imports PFBridgeCore.PFWebsocketAPI.Model
Namespace Ws
    Public Class Connection
        Implements IBridgeMCBase
        Public Sub RunCmd(cmd As String) Implements IBridgeMCBase.RunCmd
            Dim packet1 = New ActionRunCmd(cmd, "", Nothing)
            Dim packet2 = New EncryptedPack(EncryptionMode.AES256, packet1.ToString(), Token)
            Client.Send(packet2.ToString)
        End Sub
        Public Sub RunCmd(cmd As String, callback As Action(Of String)) Implements IBridgeMCBase.RunCmdCallback
            Dim packet1 = New ActionRunCmd(cmd, Rnd() * Integer.MaxValue, Nothing)
            CmdQueue.Add(New WaitingModel(packet1, callback))
            Dim packet2 = New EncryptedPack(EncryptionMode.AES256, packet1.ToString(), Token)
            Client.Send(packet2.ToString)
        End Sub
        Public Sub New(url As String, _token As String)
            Client = New WebSocket(url)
            Token = _token
            AddHandler Client.OnMessage, Sub(sender, e)
                                             ProcessMessage(Me, e.Data)
                                         End Sub
            AddHandler Client.OnOpen, Sub(sender, e)
                                          API.Log($"与{Client.Url}的连接已建立 ")
                                      End Sub
            AddHandler Client.OnError, Sub(sender, e)
                                           API.LogErr($"Websocket实例遇到错误：{ e.Exception}")
                                       End Sub
            AddHandler Client.OnClose, Sub(sender, e)
                                           API.Log("断开连接，将自动尝试重连： " & e.Reason)
                                       End Sub
            AddHandler CheckTimer.Elapsed, Sub(sender, e)
                                               CheckConnect()
                                           End Sub
            Client.ConnectAsync()
            CheckTimer.Start()
        End Sub
        Property Client As WebSocket
        Property Token As String
        Private CheckTimer As New Timers.Timer(10000)
        Private Sub CheckConnect()
            If Not Client.IsAlive Then
                Client.ConnectAsync()
            End If
        End Sub


    End Class
End Namespace
