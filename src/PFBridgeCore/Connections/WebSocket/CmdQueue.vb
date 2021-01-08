Imports PFBridgeCore.PFWebsocketAPI.Model
Friend Module Cmds
    Friend CmdQueue As New List(Of WaitingModel)
    Friend Class WaitingModel
        Public Sub New(_send As ActionRunCmd, _callback As Action(Of String))
            Callback = _callback : Send = _send
            Timeout = New Timers.Timer(5000)
            AddHandler Timeout.Elapsed, Sub(sender, e)
                                            GetFeedback(Nothing)
                                        End Sub
            Timeout.Start()
        End Sub
        Friend Send As ActionRunCmd
        Friend Timeout As Timers.Timer
        Friend Callback As Action(Of String)
        Friend Sub GetFeedback(fb As String)
            Try : Timeout.Stop() : Timeout.Dispose() : Catch : End Try
            Try : Callback.Invoke(fb) : Catch : End Try
            Try : CmdQueue.Remove(Me) : Finalize() : Catch : End Try
        End Sub
    End Class
    Friend Sub ProcessCmdFeedback(pack As CauseRuncmdFeedback)
        With pack.params
            If String.IsNullOrEmpty(.id) Then Return
            Dim i = CmdQueue.FindIndex(Function(l) l.Send.params.id = .id)
            If i <> -1 Then
                CmdQueue(i).GetFeedback(.result)
            End If
        End With
    End Sub
End Module
