
Friend Module ConnectionManager
    Public Sub AddWebsocketClient(url As String, token As String)
        MCConnections.Add(New Ws.Connection(url, token))
#If DEBUG Then
        API.Log($"添加WebsocketClient实例成功！({url})")
#End If
    End Sub
    'Friend Function GetClient()
    'End Function
End Module