Imports PFBridgeCore.EventArgs
Public Module APIs
    Public API As IBridgeQQBase
    Public Events As New EventsMap
    Public Class EventsMap
        Public QQ As New QQEventsMap
        Public Server As New ServerEventsMap
#Region "QQ"
        Public Class QQEventsMap
#Region "群消息"
            Public OnGroupMessage As New EventsMapItem(Of GroupMessageEventsArgs)
            Public Class GroupMessageEventsArgs
                Inherits BaseEventsArgs
                Public Sub New(_FromGroup As Long, _FromQQ As Long, msg As String)
                    FromGroup = _FromGroup : FromQQ = _FromQQ : Message = msg
                End Sub
                Public Property FromGroup As Long
                Public Property FromQQ As Long
                Public Property Message As String
            End Class
#End Region
        End Class
#End Region
#Region "Server"
        Public Class ServerEventsMap
            Public Class ServerBaseEventsArgs
                Inherits BaseEventsArgs
                Public Sub New(_con As IBridgeMCBase)
                    Connection = _con
                End Sub
                Public Connection As IBridgeMCBase
            End Class
#Region "玩家加入"
            Public Join As New EventsMapItem(Of ServerPlayerJoinEventsArgs)
            Public Class ServerPlayerJoinEventsArgs
                Inherits ServerBaseEventsArgs
                Public Sub New(_con As IBridgeMCBase, _Sender As String, _IP As String, _UUID As String, _XUID As String)
                    MyBase.New(_con)
                    Sender = _Sender : IP = _IP : UUID = _UUID : XUID = _XUID
                End Sub
                Public Property Sender As String
                Public Property IP As String
                Public Property UUID As String
                Public Property XUID As String
            End Class
#End Region
#Region "玩家离开"
            Public Left As New EventsMapItem(Of ServerPlayerLeftEventsArgs)
            Public Class ServerPlayerLeftEventsArgs
                Inherits ServerBaseEventsArgs
                Public Sub New(_con As IBridgeMCBase, _Sender As String, _IP As String, _UUID As String, _XUID As String)
                    MyBase.New(_con)
                    Sender = _Sender : IP = _IP : UUID = _UUID : XUID = _XUID
                End Sub
                Public Property Sender As String
                Public Property IP As String
                Public Property UUID As String
                Public Property XUID As String
            End Class
#End Region
#Region "玩家消息"
            Public Chat As New EventsMapItem(Of ServerPlayerChatEventsArgs)
            Public Class ServerPlayerChatEventsArgs
                Inherits ServerBaseEventsArgs
                Public Sub New(_con As IBridgeMCBase, _Sender As String, _Message As String)
                    MyBase.New(_con)
                    Sender = _Sender : Message = _Message
                End Sub
                Public Property Sender As String
                Public Property Message As String
            End Class
#End Region
#Region "玩家命令"
            Public Cmd As New EventsMapItem(Of ServerPlayerCmdEventsArgs)
            Public Class ServerPlayerCmdEventsArgs
                Inherits ServerBaseEventsArgs
                Public Sub New(_con As IBridgeMCBase, _Sender As String, _Cmd As String)
                    MyBase.New(_con)
                    Sender = _Sender : Cmd = _Cmd
                End Sub
                Public Property Sender As String
                Public Property Cmd As String

            End Class
#End Region
        End Class
#End Region
    End Class
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

