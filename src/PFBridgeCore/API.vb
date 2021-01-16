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
                Public Sub New(_FromGroup As Long, _FromQQ As Long, msg As String, _getGroupName As Func(Of String), _getQQNick As Func(Of String), _getQQCard As Func(Of String))
                    fromGroup = _FromGroup : fromQQ = _FromQQ : message = msg
                    getGroupName = _getGroupName : getQQCard = _getQQCard : getQQNick = _getQQNick
                End Sub
                Public ReadOnly Property fromGroup As Long
                Public ReadOnly Property fromGroupName As String
                    Get
                        Return getGroupName.Invoke
                    End Get
                End Property
                Public ReadOnly Property fromQQ As Long
                Public ReadOnly Property fromQQNick As String
                    Get
                        Return getQQNick.Invoke
                    End Get
                End Property
                Public ReadOnly Property fromQQCard As String
                    Get
                        Return getQQCard.Invoke
                    End Get
                End Property
                Public ReadOnly Property message As String
                Public Property getGroupName As Func(Of String)
                Public Property getQQNick As Func(Of String)
                Public Property getQQCard As Func(Of String)
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