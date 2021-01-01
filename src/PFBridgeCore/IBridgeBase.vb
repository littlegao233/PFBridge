Imports WebSocketSharp
Public Interface IBridgeQQBase
    Sub SendGroupMessage(TargetGroup As Long, Message As String)
    Sub SendPrivateMessageFromGroup(TargetGroup As Long, QQid As Long, Message As String)
    Sub Log(Message As Object)
    Sub LogErr(Message As Object)
    ReadOnly Property PluginDataPath As String
End Interface
Public Interface IBridgeMCBase
    Sub RunCmd(cmd As String)
End Interface