Imports System

Namespace PFBridgeToCQ
    Friend Class test
        Public Sub test1()
            Using ws = New WebSocket("ws://dragonsnest.far/Laputa")
                ws.OnMessage += Sub(sender, e) Console.WriteLine("Laputa says: " & e.Data)
                ws.Connect()
                ws.Send("BALUS")
                Console.ReadKey(True)
            End Using
        End Sub
    End Class
End Namespace
