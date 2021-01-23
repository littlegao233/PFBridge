Imports Sora.Server
Public Module AppMain
    Public Sub Main(args() As String)
        Dim server As New SoraWSServer(New ServerConfig())
        server.StartServer()
        Do While True
            Console.WriteLine(Console.ReadLine() + "?")

        Loop
    End Sub
End Module
