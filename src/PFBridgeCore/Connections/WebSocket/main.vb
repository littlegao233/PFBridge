Imports Newtonsoft.Json.Linq
Imports PFBridgeCore.APIs.EventsMap.MCEventsMap
Imports PFBridgeCore.PFWebsocketAPI.Model
Namespace Ws
    Public Module WebsocketCore
        Public Sub ProcessMessage(con As Connection, message As String)
#If DEBUG Then
            API.Log("receive:" & message)
#End If
            Try
                Dim receive = JObject.Parse(message)
                Select Case [Enum].Parse(GetType(PackType), receive("type").ToString(), True)
                    Case PackType.pack
                        Select Case [Enum].Parse(GetType(ServerCauseType), receive("cause").ToString(), True)
                            Case ServerCauseType.join
                                With New CauseJoin(receive).params
                                    Events.MC.Join.Invoke(New ServerPlayerJoinEventsArgs(con, .sender, .ip, .uuid, .xuid))
                                End With
                            Case ServerCauseType.left
                                With New CauseLeft(receive).params
                                    Events.MC.Left.Invoke(New ServerPlayerLeftEventsArgs(con, .sender, .ip, .uuid, .xuid))
                                End With
                            Case ServerCauseType.chat
                                With New CauseChat(receive).params
                                    Events.MC.Chat.Invoke(New ServerPlayerChatEventsArgs(con, .sender, .text))
                                End With
                            Case ServerCauseType.cmd
                                With New CauseCmd(receive).params
                                    Events.MC.Cmd.Invoke(New ServerPlayerCmdEventsArgs(con, .sender, .text))
                                End With
                            Case ServerCauseType.mobdie
                                With New CauseMobDie(receive).params
                                    Events.MC.MobDie.Invoke(New ServerMobDieEventsArgs(con, .mobname, .mobtype, .dmcase, .srcname, .srctype, .pos))
                                End With
                            Case ServerCauseType.runcmdfeedback '命令返回
                                ProcessCmdFeedback(New CauseRuncmdFeedback(receive))
                            Case ServerCauseType.decodefailed '解码失败的包
                                Dim e As CauseDecodeFailed = New CauseDecodeFailed(receive)
                                API.LogErr(con.Client.Url.ToString & "==>" & e.params.msg)
                            Case ServerCauseType.invalidrequest '请求无效的包
                                Dim e As CauseInvalidRequest = New CauseInvalidRequest(receive)
                                API.LogErr(con.Client.Url.ToString & "==>" & e.params.msg)
                        End Select
                    Case PackType.encrypted
#If DEBUG Then
                        API.Log("收到加密包")
#End If
                        Dim ep As EncryptedPack = New EncryptedPack(receive)
                        'Select Case ep.params.mode
                        'Case EncryptionMode.aes256
                        Dim passwd As String = con.Token
                        Dim decoded As String = ep.Decode(passwd)
                        ProcessMessage(con, decoded)
                        'Case EncryptionMode.aes_cbc_pck7padding
                        'End Select
                    Case Else
                End Select
            Catch
            End Try
        End Sub
    End Module
End Namespace
