Namespace Utils
    Namespace Net.Sockets

        Public Module Socket
            Public Function CreateSocket(a As Integer, b As Integer)
                Return New System.Net.Sockets.Socket(a, b)
            End Function
            Public Function CreateSocket(a As Integer, b As Integer, c As Integer)
                Dim aa = (New System.Net.Sockets.Socket(a, b, c))
                Dim bb As Array(Of Byte)

                Return New System.Net.Sockets.Socket(a, b, c)

            End Function
        End Module
    End Namespace
End Namespace
