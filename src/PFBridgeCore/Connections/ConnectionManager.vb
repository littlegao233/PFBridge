
Friend Module ConnectionManager
    Public Sub AddWebsocketClient(url As String, token As String)
        MCConnections.Add(New Ws.Connection(url, token))
    End Sub
    'Friend Function GetClient()
    'End Function
End Module