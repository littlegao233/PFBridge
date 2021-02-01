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
                Public Sub New(_Group As Long, _QQ As Long, msg As String, _getGroupName As Func(Of String), _getQQNick As Func(Of String), _getQQCard As Func(Of String), _Feedback As Action(Of String))
                    groupId = _Group : senderId = _QQ : message = msg
                    getGroupName = _getGroupName : getQQCard = _getQQCard : getQQNick = _getQQNick
                    feedback = _Feedback
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
                Public ReadOnly Property getGroupName As Func(Of String)
                Public ReadOnly Property getQQNick As Func(Of String)
                Public ReadOnly Property getQQCard As Func(Of String)
                Public ReadOnly Property feedback As Action(Of String)
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
        End Class
#End Region
    End Class
    Public Class MessageMatchCmd
        Public Sub New(msg As String)
            message = msg
        End Sub
        Private CmdList As String() = Nothing
        Private CmdStart As String() = Nothing
        Dim message As String
        Public Function getCommands(ParamArray start() As String) As String()
            If CmdStart Is Nothing OrElse CmdList Is Nothing OrElse CmdStart.Any(Function(l) Not start.Contains(l)) Then
                CmdList = New String() {}
                Dim cmd As String = message.Trim
                For Each st In start
                    If cmd.StartsWith(st) Then
                        cmd = cmd.Substring(st.Length)
                        Exit For
                    End If
                Next
                CmdList = Regex.Matches(cmd, """(.*?)""|[\S-[""]]+").OfType(Of Match)().Select(Function(m) If(m.Value(0) = """"c, m.Groups(1).Value, m.Value))
                'Dim result As Jint.Native.Array.ArrayConstructor = Jint.Native.Array.ArrayConstructor.FromObject
            End If
            Return CmdList.ToArray
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
End Namespace