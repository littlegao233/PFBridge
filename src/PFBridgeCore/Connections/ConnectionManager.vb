
Friend Module ConnectionManager
    Friend Sub AddWebsocketClient(url As String, token As String)
        MCConnections.Add(New Ws.Connection(url, token))
    End Sub
End Module
