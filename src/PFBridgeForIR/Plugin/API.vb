Imports System.IO
Imports PFBridgeCore
Imports PFBridgeForIR.IRQQ.CSharp

Namespace PFBridgeForER.Plugin
    Friend Class API
        Implements IBridgeQQBase
        Public ReadOnly Property PluginDataPath As String Implements IBridgeQQBase.PluginDataPath
            Get
                Dim path As String = IRApi.PluginDataPath
                If Not Directory.Exists(path) Then Directory.CreateDirectory(path)
                Return path
            End Get
        End Property

        Public Sub SendGroupMessage(TargetGroup As Long, Message As String) Implements IBridgeQQBase.SendGroupMessage
            IRApi.SendGroupMessage(TargetGroup, Message)
        End Sub

        Public Sub SendPrivateMessageFromGroup(TargetGroup As Long, QQid As Long, Message As String) Implements IBridgeQQBase.SendPrivateMessageFromGroup
            IRApi.SendPrivateMessageFromGroup(TargetGroup, QQid, Message)
        End Sub

        Public Sub Log(Message As Object) Implements IBridgeQQBase.Log
            IRApi.Log(Message.ToString())
        End Sub

        Public Sub LogErr(Message As Object) Implements IBridgeQQBase.LogErr
            IRApi.Log("[ERROR]" & Message.ToString())
        End Sub
    End Class
End Namespace
