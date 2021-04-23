Imports System.Net

Namespace Utils
    Namespace Net.Sockets

        Public Module Socket
            Public Function CreateSocket(a As Integer, b As Integer)
                Return New System.Net.Sockets.Socket(a, b)
            End Function
            Public Function CreateSocket(a As Integer, b As Integer, c As Integer)
                Return New System.Net.Sockets.Socket(a, b, c)
            End Function
            Public Sub SendData(client As System.Net.Sockets.Socket, data As Byte(), address As String, port As Integer)
                Dim addr As IPAddress = Nothing
                If Not IPAddress.TryParse(address, addr) Then
                    addr = Dns.GetHostAddresses(address).First()
                End If
                client.SendTo(data, New IPEndPoint(addr, port))
                'Task queryTask = Task.Run(() >=
                '{
                '    try
                '    {
                '        client.SendTo(SendData, New IPEndPoint(IPAddress.TryParse(address, out IPAddress ipAddress) ? ipAddress : Dns.GetHostAddresses(address).First(), port));
                '        client.Receive(ReceiveData, ReceiveData.Length, SocketFlags.None);
                '    }
                '    Catch (Exception) { }
                '}
                ');
                'queryTask.Wait(TimeSpan.FromSeconds(10));
                'If (!queryTask.IsCompleted || queryTask.IsFaulted) { throw New ArgumentNullException("Query Failed", "Unable to connect to the server!"); }
                'queryTask.Dispose();
                'Int i = 0;
            End Sub
            Public Function ReceiveData(client As System.Net.Sockets.Socket, size As Integer) As Byte()
                Return ReceiveData(client, size, System.Net.Sockets.SocketFlags.None)
            End Function
            Public Function ReceiveData(client As System.Net.Sockets.Socket, size As Integer, flag As Integer) As Byte()
                Dim data() As Byte = New Byte(size) {}
                client.Receive(data, size, CType(flag, System.Net.Sockets.SocketFlags))
                Return data
            End Function
            Public Function ReceiveData(client As System.Net.Sockets.Socket, offset As Integer, size As Integer, flag As Integer) As Byte()
                Dim data() As Byte = New Byte(size + offset) {}
                client.Receive(data, offset, size, CType(flag, System.Net.Sockets.SocketFlags))
                Return data
            End Function
        End Module
    End Namespace

End Namespace
