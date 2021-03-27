Imports PFBridgeCore
Imports HuajiTech.CoolQ
Imports System
Imports System.Collections.Generic

Namespace PFBridgeForCQ
    Friend Module Data
        Private GroupList As List(Of IGroup) = New List(Of IGroup)()
        Public Function GetGroup(GroupNumber As String) As IGroup
            Return GetGroup(Long.Parse(GroupNumber))
        End Function
        Public Function GetGroup(GroupNumber As Long) As IGroup
            Dim i = GroupList.FindIndex(Function(l) l.Number = GroupNumber)
            If i = -1 Then
                Dim group = CurrentPluginContext.Group(GroupNumber)
                GroupList.Add(group)
                Return group
            Else
                Return GroupList(i)
            End If
        End Function
        Public Function GetMember(GroupNumber As String, QQid As String) As IMember
            Return GetMember(Long.Parse(GroupNumber), Long.Parse(QQid))
        End Function
        Public Function GetMember(GroupNumber As Long, QQid As Long) As IMember
            Return CurrentPluginContext.Member(QQid, GroupNumber)
        End Function
    End Module
    Friend Class API
        Implements IBridgeQQBase
        Public ReadOnly Property PluginDataPath As String Implements IBridgeQQBase.PluginDataPath
            Get
                Dim path = CurrentPluginContext.Bot.AppDirectory.FullName
                If Not My.Computer.FileSystem.DirectoryExists(path) Then My.Computer.FileSystem.CreateDirectory(path)
                Return path
            End Get
        End Property
        Public Sub Log(Message As Object) Implements IBridgeQQBase.Log
            CurrentPluginContext.Logger.LogSuccess(Message.ToString())
        End Sub
        Public Sub LogErr(Message As Object) Implements IBridgeQQBase.LogErr
            Try
                CurrentPluginContext.Logger.LogWarning(Message.ToString())
            Catch ex As Exception : System.Console.WriteLine(ex) : End Try
        End Sub
        Public Sub SendGroupMessage(TargetGroup As Long, Message As String) Implements IBridgeQQBase.SendGroupMessage
            GetGroup(TargetGroup).Send(Message)
        End Sub
        Public Sub SendPrivateMessageFromGroup(TargetGroup As Long, QQid As Long, Message As String) Implements IBridgeQQBase.SendPrivateMessageFromGroup
            GetMember(TargetGroup, QQid).Send(Message)
        End Sub
        Public Property ParseMessageFormat As IParseMessageFormat = New Model.DefaultParseFormat Implements IBridgeQQBase.ParseMessageFormat
    End Class
End Namespace
