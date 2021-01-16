﻿Imports Newtonsoft.Json.Linq
Imports PFBridgeCore.EventArgs
Imports XQ.Net.SDK.Attributes
Imports XQ.Net.SDK.EventArgs
Imports PFBridgeCore.APIs.EventsMap.QQEventsMap

<Plugin>
Friend Module Main
    <GroupMsgEvent>
    Public Sub OnGroupMsg(sender As Object, e As XQAppGroupMsgEventArgs)
        PFBridgeCore.Events.QQ.OnGroupMessage.Invoke(New GroupMessageEventsArgs(e.FromGroup.Id, e.FromQQ.Id, e.Message.Text,
                                                                                                Function() e.FromGroup.GetGroupName(e.RobotQQ),
                                                                                                Function() e.FromQQ.GetNick(e.RobotQQ),
                                                                                                Function() e.FromGroup.GetGroupCard(e.RobotQQ, e.FromQQ.Id),
                                                                                                Sub(s) e.FromGroup.SendMessage(e.RobotQQ, s)
                                                                                                ))
    End Sub
    <EnableEvent>
    Public Sub OnStartup(sender As Object, e As XQEventArgs)
        GroupMsgAPI = e.XQAPI
        Try
            'Dim path As String = Environment.CurrentDirectory + "\Plugin\Temp\PFBridge.XQ.dll"
            'PFBridgeCore.AssemblyList.Add(path, Reflection.Assembly.LoadFrom(path))
            PFBridgeCore.Init(New API) '核心启动
        Catch ex As Exception
            XQ.Net.SDK.XQAPI.OutPutLog("错误:" + ex.ToString)
        End Try
        'For Each a In QQList
        '    For Each c In a
        '        XQ.Net.SDK.XQAPI.OutPutLog(a & "->" & c & ":" & Asc(c))
        '    Next
        'Next
    End Sub
    Friend ReadOnly Property QQList As List(Of String)
        Get
            Return XQ.Net.SDK.XQAPI.GetQQList.Split(New Char() {vbLf, vbCr}, StringSplitOptions.RemoveEmptyEntries).ToList
        End Get
    End Property
    Friend GroupMsgAPI As XQ.Net.SDK.XQAPI
End Module