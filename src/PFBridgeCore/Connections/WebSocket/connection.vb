'#If NETFULL Then
Imports WebSocketSharp
'#Else
'Imports sta_websocket_sharp_core
'#End If

Imports PFBridgeCore.PFWebsocketAPI.Model

Namespace Ws
    Public Class Connection
        Implements IBridgeMCBase
        Public Sub RunCmd(cmd As String) Implements IBridgeMCBase.RunCmd
            Try
                Dim packet1 As ActionRunCmd = New ActionRunCmd(cmd, "", Nothing)
                Dim packet2 = New EncryptedPack(EncryptionMode, packet1.ToString(), Token)
                If Client.IsAlive Then
                    Client.SendAsync(packet2.ToString, Sub(result)
                                                       End Sub)
                Else
                    CheckConnect()
                    If Not CheckTimer.Enabled Then CheckTimer.Start()
                End If
            Catch ex As Exception
                API.LogErr(ex)
            End Try
        End Sub
        Public Sub RunCmd(cmd As String, callback As Action(Of String)) Implements IBridgeMCBase.RunCmdCallback
            Dim packet1 = New ActionRunCmd(cmd, Guid.NewGuid().ToString(), Nothing)
            CmdQueue.Add(New WaitingModel(packet1, callback))
            Dim packet2 = New EncryptedPack(EncryptionMode, packet1.ToString(), Token)
            If Client.IsAlive Then
                Client.SendAsync(packet2.ToString, Sub(result)
                                                   End Sub)
            Else
                CheckConnect()
                If Not CheckTimer.Enabled Then CheckTimer.Start()
            End If
        End Sub
        Private ReadOnly WSUrl As String
        Private Function CreateNewClient() As WebSocket
            Dim result = New WebSocket(WSUrl)
            result.WaitTime = TimeSpan.FromSeconds(5)
            result.Log.Output = Sub(data, e)
                                    Select Case data.Level
                                        Case LogLevel.Trace, LogLevel.Debug, LogLevel.Info
                                            If data.Message.ToLower = "the connection has already been closed." Then Return
                                            API.Log($"{result.Url}[ws|{data.Level}]{data.Message}")
                                        Case LogLevel.Fatal
#If DEBUG Then
                                            API.Log(data.Caller.GetMethod().Name)
#End If
                                            If data.Caller.GetMethod().Name = "<startReceiving>b__2" Then Return
                                            If data.Message.ToLower.StartsWith("no connection could be made because the target machine actively refused it.") Then API.LogErr($"{Client.Url}[ws|{data.Level}]无法建立连接，因为目标计算机主动拒绝了该连接") : Return
                                            If data.Message.StartsWith("System.NullReferenceException") Then Return
                                            API.LogErr($"{result.Url}[ws|{data.Level}]{data.Message}")
                                        Case LogLevel.Error, LogLevel.Warn
                                            If data.Message.ToLower = "the current output action has been changed." Then Return
                                            If data.Message.ToLower = "the current logging level has been changed to info." Then Return
                                            API.LogErr($"{result.Url}[ws|{data.Level}]{data.Message}")
                                    End Select
                                End Sub
            result.Log.Level = LogLevel.Info
            Return result
        End Function
        Public Sub New(url As String, _token As String, _tag As Object)
            Id = IdAll : IdAll += 1
            Tag = _tag
            Token = _token
            WSUrl = url
            Client = CreateNewClient()
            AddHandler Client.OnMessage, Sub(sender, e)
                                             ProcessMessage(Me, e.Data)
                                         End Sub
            AddHandler Client.OnOpen, Sub(sender, e)
                                          ConnectionState = Client.IsAlive
                                      End Sub
            'AddHandler Client.OnError, Sub(sender, e)
            '                               API.LogErr($"{Client.Url}遇到错误：{ e.Exception}")
            '                           End Sub
            AddHandler Client.OnClose, Sub(sender, e)
                                           If e.Code = 1006 Then
                                               API.LogErr($"{Client.Url}建立连接失败[" & e.Code & ":连接状态异常]，将自动尝试重连“)
                                           Else
                                               API.LogErr($"{Client.Url}断开连接，将自动尝试重连[" & e.Code & "]:" & e.Reason)
                                           End If
                                       End Sub
            AddHandler CheckTimer.Elapsed, Sub(sender, e)
                                               Try
                                                   CheckConnect()
                                               Catch : End Try
                                           End Sub
        End Sub
        Public Property Client As WebSocket
        Public Property Id As Integer Implements IBridgeMCBase.Id
        Private Shared IdAll As Integer = 0
        Public Property Token As String

        Public Property Tag As Object Implements IBridgeMCBase.Tag
#If DEBUG Then
        Private ReadOnly CheckTimer As New Timers.Timer() With {.AutoReset = True, .Interval = 1000, .Enabled = True}
#Else
        Private ReadOnly CheckTimer As New Timers.Timer() With {.AutoReset = True, .Interval = 10000, .Enabled = True}
#End If
        'Private ConnectingStateTimes As Integer = 0
        Private ClosedStateTimes As Integer = 0
        Private _connectionState As Boolean = False
        Private WriteOnly Property ConnectionState
            Set(value)
                If _connectionState <> value Then
                    If value Then
                        If Client.Ping() Then
                            API.Log($"与{Client.Url}的连接已建立 ")
                            _connectionState = value
                            'ConnectingStateTimes = 0
                            ClosedStateTimes = 0
                        End If
                    Else
                        _connectionState = value
                    End If
                End If
            End Set
        End Property

        Private Sub CheckConnect() Implements IBridgeMCBase.CheckConnect
            Try
                If Not Client.IsAlive Then
#If DEBUG Then
                    API.Log(Client.ReadyState)
#End If
                    If Client.ReadyState = WebSocketState.Closed OrElse Client.ReadyState = WebSocketState.Connecting Then
                        Client.Connect()
                        ClosedStateTimes += 1
                        If ClosedStateTimes > 30 Then
                            Try : Client.Log.Output = Sub()
                                                      End Sub : Catch : End Try
                            Try : Client.CloseAsync() : Catch : End Try
                            Client = CreateNewClient()
                            ClosedStateTimes = 0
                        End If
                        'ElseIfThen
                        'ConnectingStateTimes += 1
                        'If ConnectingStateTimes > 4 Then
                        '    Client.ConnectAsync()
                        '    ConnectingStateTimes = 0
                        'End If
                    End If
                End If
            Catch ex As InvalidOperationException
                Try : Client.Log.Output = Sub()
                                          End Sub : Catch : End Try
                Try : Client.CloseAsync() : Catch : End Try
                Client = CreateNewClient()
            Catch ex As Exception
#If DEBUG Then
                API.LogErr(ex.ToString)
#End If
            End Try
            Try
                ConnectionState = Client.IsAlive
            Catch : End Try
        End Sub
        Public Shared Property EncryptionMode As EncryptionMode = EncryptionMode.aes_cbc_pck7padding

        Public ReadOnly Property State As Boolean Implements IBridgeMCBase.State
            Get
                Return Client.IsAlive
            End Get
        End Property
    End Class
End Namespace
