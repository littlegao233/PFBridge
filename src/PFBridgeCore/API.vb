Friend Module QQAPI
    Friend API As IBridgeBase
    Friend Events As New EventsMap
    Friend Class EventsMap
        Public OnGroupMessage As New List(Of Action)
    End Class
End Module
