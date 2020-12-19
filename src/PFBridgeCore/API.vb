Public Module QQAPI
    Public API As IBridgeBase
    Public Events As New EventsMap
    Public Class EventsMap
        Public OnGroupMessage As New EventsMapItem
    End Class
    Public Class EventsMapItem
        Inherits List(Of Action(Of Object))
        Public Sub Invoke(Args As Object)
            ForEach(Sub(l) l.Invoke(Args))
        End Sub
    End Class
End Module
