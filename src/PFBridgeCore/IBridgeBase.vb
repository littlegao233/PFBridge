Imports PFBridgeCore.EventArgs
Imports WebSocketSharp
Public Interface IBridgeQQBase
    Sub SendGroupMessage(TargetGroup As Long, Message As String)
    Sub SendPrivateMessageFromGroup(TargetGroup As Long, QQid As Long, Message As String)
    Sub Log(Message As Object)
    Sub LogErr(Message As Object)
    ReadOnly Property PluginDataPath As String
    'Function GetGroupMemberInfo(TargetGroup As Long, QQid As Long) As GroupMemberInfo
End Interface
Public Interface IBridgeMCBase
    Property Id As Integer
    Property Tag As Object
    ReadOnly Property State As Boolean
    Sub RunCmdCallback(cmd As String, callback As Action(Of String))
    Sub RunCmd(cmd As String)
    Sub CheckConnect()
End Interface