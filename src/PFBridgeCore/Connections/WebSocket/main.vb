Imports WebSocketSharp

Public Class WebsocketCore
    Public Sub New()
        Using ws = New WebSocket("ws://dragonsnest.far/Laputa")
            AddHandler ws.OnMessage, Sub(sender, e) Console.WriteLine("Laputa says: " & e.Data)
            ws.Connect()
            ws.Send("BALUS")
            Console.ReadKey(True)

        End Using
    End Sub
End Class
