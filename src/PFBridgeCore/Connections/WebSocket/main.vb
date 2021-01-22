Imports Newtonsoft.Json.Linq
Imports PFBridgeCore.APIs.EventsMap.ServerEventsMap
Imports PFBridgeCore.PFWebsocketAPI.Model
Namespace Ws
    Public Module WebsocketCore
        Public Sub ProcessMessage(con As Connection, message As String)
            Try
                Dim receive = JObject.Parse(message)
                Select Case [Enum].Parse(GetType(PackType), receive("type").ToString())
                    Case PackType.pack
                        Select Case [Enum].Parse(GetType(ServerCauseType), receive("cause").ToString())
                            Case ServerCauseType.join
                                With New CauseJoin(receive).params
                                    Events.Server.Join.Invoke(New ServerPlayerJoinEventsArgs(con, .sender, .ip, .uuid, .xuid))
                                End With
                            Case ServerCauseType.left
                                With New CauseLeft(receive).params
                                    Events.Server.Left.Invoke(New ServerPlayerLeftEventsArgs(con, .sender, .ip, .uuid, .xuid))
                                End With
                            Case ServerCauseType.chat
                                With New CauseChat(receive).params
                                    Events.Server.Chat.Invoke(New ServerPlayerChatEventsArgs(con, .sender, .text))
                                End With
                            Case ServerCauseType.cmd
                                With New CauseCmd(receive).params
                                    Events.Server.Cmd.Invoke(New ServerPlayerCmdEventsArgs(con, .sender, .text))
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
                        Dim ep As EncryptedPack = New EncryptedPack(receive)
                        Select Case ep.params.mode
                            Case EncryptionMode.AES256
                                Dim passwd As String = con.Token
                                Dim decoded As String = ep.Decode(passwd)
                                ProcessMessage(con, decoded)
                        End Select
                    Case Else
                End Select
            Catch
            End Try
        End Sub
    End Module
End Namespace
