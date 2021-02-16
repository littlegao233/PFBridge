Imports System.Text.RegularExpressions
Imports PFBridgeCore.EventArgs
Public Module APIs
    Public Property API As IBridgeQQBase
    Public Property Events As New EventsMap
#Disable Warning IDE1006 ' 命名样式
    Public Class EventsMap
        Public QQ As New QQEventsMap
        Public Server As New ServerEventsMap
#Region "QQ"
        Public Class QQEventsMap
#Region "群消息"
            Public OnGroupMessage As New EventsMapItem(Of GroupMessageEventsArgs)
            Public Class GroupMessageEventsArgs
                Inherits BaseEventsArgs
                Public Sub New(_Group As Long, _QQ As Long, msg As String, _getGroupName As Func(Of String), _getQQNick As Func(Of String), _getQQCard As Func(Of String), _getMemberType As Func(Of Integer), _Feedback As Action(Of String), _parseMessagef As Func(Of String))
                    groupId = _Group : senderId = _QQ : message = msg
                    getGroupName = _getGroupName : getQQCard = _getQQCard : getQQNick = _getQQNick
                    feedback = _Feedback
                    getMemberType = _getMemberType
                    getParsedMessage = _parseMessagef
                End Sub
                Public ReadOnly Property groupId As Long
                Public ReadOnly Property groupName As String
                    Get
                        Return getGroupName.Invoke
                    End Get
                End Property
                Public ReadOnly Property senderId As Long
                Public ReadOnly Property senderNick As String
                    Get
                        Return getQQNick.Invoke
                    End Get
                End Property
                Public ReadOnly Property memberCard As String
                    Get
                        Return getQQCard.Invoke
                    End Get
                End Property
                Public ReadOnly Property memberType As String
                    Get
                        Return getMemberType.Invoke
                    End Get
                End Property
                Public ReadOnly Property message As String

                Private _MessageMatchCmd As MessageMatchCmd = Nothing
                Public ReadOnly Property messageMatch As MessageMatchCmd
                    Get
                        If _MessageMatchCmd Is Nothing Then
                            _MessageMatchCmd = New MessageMatchCmd(message)
                        End If
                        Return _MessageMatchCmd
                    End Get
                End Property
                Private _ParsedMessage As String = Nothing
                Public ReadOnly Property parsedMessage As String
                    Get
                        If _ParsedMessage Is Nothing Then _ParsedMessage = getParsedMessage.Invoke()
                        Return _ParsedMessage
                    End Get
                End Property
                Public ReadOnly Property getGroupName As Func(Of String)
                Public ReadOnly Property getQQNick As Func(Of String)
                Public ReadOnly Property getQQCard As Func(Of String)
                Public ReadOnly Property getMemberType As Func(Of Integer)
                Public ReadOnly Property feedback As Action(Of String)
                Public ReadOnly Property getParsedMessage As Func(Of String)
            End Class
#End Region
        End Class
#End Region
#Region "Server"
        Public Class ServerEventsMap
            Public Class ServerBaseEventsArgs
                Inherits BaseEventsArgs
                Public Sub New(_con As IBridgeMCBase)
                    connection = _con
                End Sub
                Public connection As IBridgeMCBase
            End Class
#Region "玩家加入"
            Public Join As New EventsMapItem(Of ServerPlayerJoinEventsArgs)
            Public Class ServerPlayerJoinEventsArgs
                Inherits ServerBaseEventsArgs
                Public Sub New(_con As IBridgeMCBase, _Sender As String, _IP As String, _UUID As String, _XUID As String)
                    MyBase.New(_con)
                    sender = _Sender : ip = _IP : uuid = _UUID : xuid = _XUID
                End Sub
                Public ReadOnly Property sender As String
                Public ReadOnly Property ip As String
                Public ReadOnly Property uuid As String
                Public ReadOnly Property xuid As String
            End Class
#End Region
#Region "玩家离开"
            Public Left As New EventsMapItem(Of ServerPlayerLeftEventsArgs)
            Public Class ServerPlayerLeftEventsArgs
                Inherits ServerBaseEventsArgs
                Public Sub New(_con As IBridgeMCBase, _Sender As String, _IP As String, _UUID As String, _XUID As String)
                    MyBase.New(_con)
                    sender = _Sender : ip = _IP : uuid = _UUID : xuid = _XUID
                End Sub
                Public ReadOnly Property sender As String
                Public ReadOnly Property ip As String
                Public ReadOnly Property uuid As String
                Public ReadOnly Property xuid As String
            End Class
#End Region
#Region "玩家消息"
            Public Chat As New EventsMapItem(Of ServerPlayerChatEventsArgs)
            Public Class ServerPlayerChatEventsArgs
                Inherits ServerBaseEventsArgs
                Public Sub New(_con As IBridgeMCBase, _Sender As String, _Message As String)
                    MyBase.New(_con)
                    sender = _Sender : message = _Message
                End Sub
                Public ReadOnly Property sender As String
                Public ReadOnly Property message As String
            End Class
#End Region
#Region "玩家命令"
            Public Cmd As New EventsMapItem(Of ServerPlayerCmdEventsArgs)
            Public Class ServerPlayerCmdEventsArgs
                Inherits ServerBaseEventsArgs
                Public Sub New(_con As IBridgeMCBase, _Sender As String, _Cmd As String)
                    MyBase.New(_con)
                    sender = _Sender : cmd = _Cmd
                End Sub
                Public ReadOnly Property sender As String
                Public ReadOnly Property cmd As String

            End Class
#End Region
#Region "生物死亡"
            Public MobDie As New EventsMapItem(Of ServerMobDieEventsArgs)
            Public Class ServerMobDieEventsArgs
                Inherits ServerBaseEventsArgs
                Public Sub New(_con As IBridgeMCBase, _mobname As String, _mobtype As String, _dmcase As Integer, _srcname As String, _srctype As String, _pos As PFWebsocketAPI.Model.Vec3)
                    MyBase.New(_con)
                    mobname = _mobname : mobtype = _mobtype : dmcase = _dmcase
                    srctype = _srctype : srcname = _srcname : pos = _pos
                End Sub
                Public ReadOnly Property mobname As String
                Public ReadOnly Property mobtype As String
                Public ReadOnly Property dmcase As Integer
                Public ReadOnly Property srcname As String
                Public ReadOnly Property srctype As String
                Public ReadOnly Property pos As PFWebsocketAPI.Model.Vec3
            End Class
#End Region
        End Class
#End Region
    End Class
    Public Class MessageMatchCmd
        Public Sub New(msg As String)
            message = msg
        End Sub
        Private CmdList As List(Of String) = Nothing
        Private CmdStart As String() = Nothing
        Dim message As String
        Public Function getCommands(ParamArray start() As String) As List(Of String)
            Try
                If CmdStart Is Nothing OrElse CmdList Is Nothing OrElse Not CmdStart.All(Function(l) start.Contains(l)) Then
                    CmdStart = start
                    Dim cmd As String = message.Trim
                    For Each st In start
                        If cmd.StartsWith(st) Then
                            cmd = cmd.Substring(st.Length)
                            Exit For
                        End If
                    Next
                    CmdList = Regex.Matches(cmd, "(?<!\\)""(.*?)(?<!\\)""|(?<!\\)'(.*?)(?<!\\)'|[\S-[""]]+").OfType(Of Match)().ToList.ConvertAll(Function(m)
                                                                                                                                                      If m.Value(0) = """"c Then
                                                                                                                                                          Return m.Groups(1).Value.Replace("\""", """")
                                                                                                                                                      ElseIf m.Value(0) = "'"c Then
                                                                                                                                                          Return m.Groups(2).Value.Replace("\'", "'")
                                                                                                                                                      End If
                                                                                                                                                      Return m.Value
                                                                                                                                                  End Function)
#If DEBUG Then
                    For Each x In CmdList
                        Console.WriteLine(x.ToString())
                    Next
#End If
                End If
            Catch ex As Exception
                APIs.API.LogErr(ex.ToString)
            End Try
            Return CmdList
        End Function
    End Class
#Enable Warning IDE1006 ' 命名样式
    Public Class EventsMapItem(Of T)
        Inherits List(Of Action(Of T))
        Public Sub Invoke(Args As T)
            ForEach(Sub(l)
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
    'Public Class GroupMemberInfo
    '    Sub New(_Permissions)
    '        Permissions = _Permissions
    '    End Sub
    '    Public Permissions As Integer
    'End Class
End Namespace
Namespace Model
    Public Class DefaultParseFormat
        Implements IParseMessageFormat
        Public Property At As String = "§r§l§6@§r§6{0}§a" Implements IParseMessageFormat.At
        Public Property AtAll As String = "§r§l§g@§r§g全体成员§a" Implements IParseMessageFormat.AtAll
        Public Property Image As String = "§r§l§d[图骗]§r§a" Implements IParseMessageFormat.Image
        Public Property Emoji As String = "§r§l§d[emoji]§r§a" Implements IParseMessageFormat.Emoji
        Public Property Face As String = "§r§l§c[表情]§r§a" Implements IParseMessageFormat.Face
        '[CQ:face,id=123]
        Public Property Bface As String = "§r§l§d[大表情:§r§o§7{0}§r§l§d]§r§a" Implements IParseMessageFormat.Bface
        Public Property Record As String = "§r§l§g[语音]§r§a" Implements IParseMessageFormat.Record
        Public Property Video As String = "§r§l§b[视频]§r§a" Implements IParseMessageFormat.Video
        Public Property Share As String = "§r§l§b[分享§r§e:{1}§d({0})§l§b]§r§a" Implements IParseMessageFormat.Share
        '[CQ:share,url=http://baidu.com,title=百度]
        Public Property Music As String = "§r§l§d[音乐§r§d:{1}§l§b]§r§a" Implements IParseMessageFormat.Music
        Public Property Reply As String = "§r§l§7[回复]§r§a" Implements IParseMessageFormat.Reply
        Public Property Forward As String = "§r§l§7[转发]§r§a" Implements IParseMessageFormat.Forward
        Public Property Node As String = "§r§l§7[转发节点]§r§a" Implements IParseMessageFormat.Node
        Public Property Xml As String = "§r§l§7[富文本消息]§r§a" Implements IParseMessageFormat.Xml
        Public Property Json As String = "§r§l§7[富文本消息]§r§a" Implements IParseMessageFormat.Json
        Public Property Unknown As String = "§r§l§7[{0}]§r§a" Implements IParseMessageFormat.Unknown
        'Public Property Data As String Implements IBridgeQQBase.IParseMessageFormat.Image
    End Class
End Namespace