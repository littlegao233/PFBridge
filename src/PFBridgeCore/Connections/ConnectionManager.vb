
Public Module ConnectionManager
    Public Function AddWebsocketClient(url As String, token As String) As Integer
        Dim client = New Ws.Connection(url, token)
        MCConnections.Add(client)
#If DEBUG Then
        API.Log($"添加WebsocketClient实例成功！({url})")
#End If
        Return client.Id
    End Function
    'Friend Function GetClient()
    'End Function
End Module