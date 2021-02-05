Imports System
Imports PFBridgeCore.EventArgs
Imports PFBridgeCore.APIs.EventsMap.QQEventsMap

Namespace PFBridgeForER.Plugin
    Friend Module App
        Private hasStarted As Boolean = False

        Friend Sub OnStartup()
            If Not hasStarted Then
                hasStarted = True
                PFBridgeCore.Main.Init(New API())
            End If
            'RefreshQQList();
        End Sub

        Private counter As Byte = 254
        ''' <summary>
        ''' 处理消息接收事件。
        ''' </summary>
        Friend Sub OnMessageReceived(ByVal group As String, ByVal sender As String, ByVal message As String, ByVal _robotQQ As String)
            'if (++counter == byte.MaxValue) { counter = byte.MinValue; App.RefreshQQList(); }
            Try
                PFBridgeCore.APIs.Events.QQ.OnGroupMessage.Invoke(New GroupMessageEventsArgs(Long.Parse(group), Long.Parse(sender), message,
                                                                                             Function() IRQQ.CSharp.IRApi.GetGroupName(_robotQQ, group),
                                                                                             Function() IRQQ.CSharp.IRApi.GetNickName(_robotQQ, sender),
                                                                                             Function() IRQQ.CSharp.IRApi.GetMemberCard(_robotQQ, group, sender),
                                                                                             Function()
                                                                                                 Dim result = 1
                                                                                                 Dim isOwner = True
                                                                                                 For Each member In IRQQ.CSharp.IRApi.GetGroupAdmin(_robotQQ, group).Split(New String() {vbCrLf}, StringSplitOptions.RemoveEmptyEntries)
                                                                                                     If member = sender Then
                                                                                                         result = If(isOwner, 3, 2)
                                                                                                         Exit For
                                                                                                     End If
                                                                                                     isOwner = False
                                                                                                 Next
                                                                                                 Return result
                                                                                             End Function,
                                                                                             Sub(s) IRQQ.CSharp.IRApi.SendGroupMessage(group, $"[IR:at={sender}]{s}")))
                'https://gitee.com/jiguang_aurora/CleverQQ-SDK/wikis/%E5%8F%98%E9%87%8F%E5%88%97%E8%A1%A8?sort_id=1516257
            Catch ex As Exception
                PFBridgeCore.APIs.API.LogErr(ex)
            End Try
        End Sub
        'private static List<long> QQList = new List<long>();
        'internal static void RefreshQQList()
        '{
        '    var obj = Newtonsoft.Json.Linq.JObject.Parse(SDK.Common.xlzAPI.GetThisQQ())["QQlist"];
        '    QQList = obj.ToList().ConvertAll(l => long.Parse(((Newtonsoft.Json.Linq.JProperty)l).Name));
        '}
        'internal static void ForEachQQ(Action<long> action)
        '{
        '    QQList.ForEach(action);
        '}
    End Module
End Namespace
