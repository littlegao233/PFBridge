Imports PFBridgeCore.EventArgs
Public Module QQAPI
    Public API As IBridgeQQBase
    Public Events As New EventsMap
    Public Class EventsMap
        Public OnGroupMessage As New EventsMapItem
    End Class
    Public Class EventsMapItem
        Inherits List(Of Action(Of BaseEventsArgs))
        Public Sub Invoke(Args As BaseEventsArgs)
            ForEach(Sub(l)
                        IO.Directory.GetFiles("").ToList()
                        Try
                            l.Invoke(Args)
                        Catch ex As Exception
                            API.LogErr($"[回调自{Args.GetType}]{ex}")
                        End Try
                    End Sub)
        End Sub
    End Class
End Module
Namespace EventArgs
    Public MustInherit Class BaseEventsArgs
    End Class
    Public Class GroupMessageEventsArgs
        Inherits BaseEventsArgs
        Public Sub New(_FromGroup As Long, _FromQQ As Long, msg As String)
            FromGroup = _FromGroup : FromQQ = _FromQQ : Message = msg
        End Sub
        Public FromGroup As Long
        Public FromQQ As Long
        Public Message As String
    End Class
End Namespace

