Imports System.IO
Imports Newtonsoft.Json.Linq

Public Module Tools
    Sub Main(args() As String)
        If args.Count <= 1 Then
            Console.WriteLine("[Error]参数不足！")
            Console.ReadKey()
            Return
        End If
        Select Case args(0).ToLower
            Case "-zip"
                Dim path As String = args(1)
            Case "-jsonremovecomment", "-jrc"
                Dim path As String = args(1)
                Threading.Thread.Sleep(1000)
                Dim raw As String = File.ReadAllText(path)
                Dim json As JToken = JToken.Parse(raw)
                If json.Type = JTokenType.Comment Then Try : json = JObject.Parse(raw) : Catch : Try : json = JArray.Parse(raw) : Catch : End Try : End Try
                File.WriteAllText(path, json.ToString(Newtonsoft.Json.Formatting.None))
                Console.WriteLine("JSON压缩完成:" & path)
            Case Else
                Console.WriteLine("[ERROR]未知操作:" & args(0))
                Console.ReadKey()
                Return
        End Select
    End Sub
End Module
