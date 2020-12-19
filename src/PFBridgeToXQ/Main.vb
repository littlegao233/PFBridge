Imports Newtonsoft.Json.Linq
Imports XQ.Net.SDK.Attributes
Imports XQ.Net.SDK.EventArgs
<Plugin>
Friend Module Main
    <GroupMsgEvent>
    Public Sub OnGroupMsg(sender As Object, e As XQAppGroupMsgEventArgs)
        'If e.RobotQQ = "3623498320" Then
        e.FromGroup.SendMessage(e.RobotQQ, e.Message.Text)
        XQ.Net.SDK.XQAPI.OutPutLog(e.Message.Text)
        'End If
    End Sub
    <EnableEvent>
    Public Sub OnStartup(sender As Object, e As XQEventArgs)
        XQ.Net.SDK.XQAPI.OutPutLog("test")
        GroupMsgAPI = e.XQAPI
        PFBridgeCore.Init(New API) '核心启动
        For Each a In QQList
            For Each c In a
                XQ.Net.SDK.XQAPI.OutPutLog(a & "->" & c & ":" & Asc(c))
            Next
        Next
    End Sub
    Friend ReadOnly Property QQList As List(Of String)
        Get
            Return XQ.Net.SDK.XQAPI.GetQQList.Split(New Char() {vbLf, vbCr}, StringSplitOptions.RemoveEmptyEntries).ToList
        End Get
    End Property
    Friend GroupMsgAPI As XQ.Net.SDK.XQAPI
End Module