Imports System.IO
Imports System.Reflection
Imports Esprima
Imports Jint
Imports Newtonsoft.Json.Linq

Public Module Main
    Public Sub Init(_api As IBridgeBase)
        API = _api '保存API
        API.Log("正在加载PFBridgeCore...")
        Dim options As Options = New Options()
        options.AllowClr()
        'options.AllowClr(GetType(Newtonsoft.Json.JsonConvert).Assembly)
        'engine.SetValue("TheType", TypeReference.CreateTypeReference(engine, TypeOf (TheType)))
        Dim engine = New Engine(options)
        engine.SetValue("api", API)
        engine.SetValue("events", Events)
        engine.Execute(JSRaw)
        'Task.Run(Sub()
        '             While True
        '                 Threading.Thread.Sleep(1000)
        '                 Events.OnGroupMessage.ForEach(Sub(l) l.Invoke())
        '             End While
        '         End Sub)
        'engine.Function.Call(New Native.JsString("hello"), New Native.JsValue() {})
        'engine.Function.c(New Native.JsString("hello"), Nothing)
    End Sub
    Public ReadOnly Property JSRaw
        Get
#If DEBUG Then
            Dim indexPath = Path.Combine(API.PluginDataPath, "index.js")
            File.Delete(indexPath)
            Return My.Resources.ResourceFiles.index
#Else
            Dim indexPath = Path.Combine(API.PluginDataPath, "index.js")
            If File.Exists(indexPath) Then
                Return File.ReadAllText(indexPath)
            Else
                Dim defaultjs As String = My.Resources.ResourceFiles.index
                File.WriteAllText(indexPath, defaultjs)
                Return defaultjs
            End If
#End If
        End Get
    End Property
End Module
