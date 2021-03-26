Imports PFBridgeCore.EventArgs
Public Interface IParseMessageFormat
    Property At As String
    Property AtAll As String
    Property Image As String
    Property Emoji As String
    Property Face As String
    Property Bface As String
    Property Record As String
    Property Video As String
    Property Share As String
    Property Music As String
    Property Reply As String
    Property Forward As String
    Property Node As String
    Property Xml As String
    Property Json As String
    Property File As String
    Property Unknown As String
End Interface
Public Interface IBridgeQQBase
    Sub SendGroupMessage(TargetGroup As Long, Message As String)
    Sub SendPrivateMessageFromGroup(TargetGroup As Long, QQid As Long, Message As String)
    Sub Log(Message As Object)
    Sub LogErr(Message As Object)
    ReadOnly Property PluginDataPath As String
    'Function GetGroupMemberInfo(TargetGroup As Long, QQid As Long) As GroupMemberInfo
    Property ParseMessageFormat As IParseMessageFormat
End Interface
Public Interface IBridgeMCBase
    Property Id As Integer
    Property Tag As Object
    ReadOnly Property State As Boolean
    Sub RunCmdCallback(cmd As String, callback As Action(Of String))
    Sub RunCmd(cmd As String)
    Sub CheckConnect()
End Interface