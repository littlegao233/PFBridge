Public Interface IBridgeBase
    Sub SendGroupMessage(TargetGroup As String, Message As String)
    Sub SendPrivateMessageFromGroup(TargetGroup As String, QQid As String, Message As String)
    Sub Log(Message As String)
    ReadOnly Property PluginDataPath As String
End Interface
