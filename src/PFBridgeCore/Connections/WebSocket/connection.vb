Imports WebSocketSharp
Namespace Ws
    Public Class Connection
        Implements IBridgeMCBase
        Public Sub RunCmd(cmd As String) Implements IBridgeMCBase.RunCmd

        End Sub
        Public Sub New(url As String, _token As String)
            Client = New WebSocket(url)
            Token = _token
            AddHandler Client.OnMessage, Sub(sender, e)
                                             ProcessMessage(Me, e.Data)
                                         End Sub
            AddHandler Client.OnClose, Sub(sender, e)
                                           Console.WriteLine("断开连接，将在10s后尝试重连： " & e.Reason)
                                           Client.Connect()
                                       End Sub
            Client.Connect()
        End Sub
        Property Client As WebSocket
        Property Token As String
    End Class
End Namespace
