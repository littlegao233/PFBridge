Imports PFBridgeCore
Imports XQ.Net.SDK
Friend Class API
    Implements IBridgeBase
    Public ReadOnly Property PluginDataPath As String Implements IBridgeBase.PluginDataPath
        Get
            Return XQAPI.AppDir
        End Get
    End Property
    Public Sub SendGroupMessage(TargetGroup As String, Message As String) Implements IBridgeBase.SendGroupMessage
        QQList.ForEach(Sub(l) XQAPI.SendMsg(l, MessageType.群聊, TargetGroup, Nothing, Message, BubbleID.跟随框架的设置))
    End Sub
    Public Sub Log(Message As String) Implements IBridgeBase.Log
        XQAPI.OutPutLog(Message)
    End Sub
    Public Sub SendPrivateMessageFromGroup(TargetGroup As String, QQid As String, Message As String) Implements IBridgeBase.SendPrivateMessageFromGroup
        QQList.ForEach(Sub(l) XQAPI.SendMsg(l, MessageType.群临时会话, TargetGroup, QQid, Message, BubbleID.跟随框架的设置))
    End Sub
End Class
Friend Module APIEx
    '信息类型 : 0在线临时会话/1好友/2群/3讨论群/4群临时会话/5讨论组临时会话/7好友验证回复会话
    Friend Enum MessageType
        临时会话 = 0
        好友 = 1
        群聊 = 2
        讨论组 = 3
        群临时会话 = 4
        讨论组临时会话 = 5
        好友验证回复会话 = 7
    End Enum
    '气泡ID(-2强制不使用气泡 -1随机使用气泡 0跟随框架的设置)
    Friend Enum BubbleID
        强制不使用气泡 = -2
        随机使用气泡 = -1
        跟随框架的设置 = 0
    End Enum
End Module
