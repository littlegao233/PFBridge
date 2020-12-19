Imports System.Reflection
Imports Newtonsoft.Json.Linq

Public Module Main
    Public Sub Init(_api As IBridgeBase)
        API = _api '保存API
        Console.WriteLine("test")
        Try
            API.Log(JObject.Parse("{'aa':'bb'}").ToString)
        Catch ex As Exception
            Console.WriteLine("test" & ex.ToString)
        End Try
        Try
            API.Log("开始加载插件...")
        Catch ex As Exception
            Console.WriteLine("test" & ex.ToString)
        End Try
    End Sub
End Module
