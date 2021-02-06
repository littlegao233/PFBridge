Imports System.IO
Imports System.Reflection
Imports Esprima
Imports Jint
Imports Newtonsoft.Json.Linq
Imports Timer = System.Timers.Timer

Public Module Main
    Public Class EngineEx
        Inherits Jint.Engine
        Public Sub New(options As Options)
            MyBase.New(options)
        End Sub
        Public Shadows Sub Execute(content As String)
            Dim rndstr As String = Guid.NewGuid.ToString.Replace("-", "_")
            MyBase.Execute($"function Main{rndstr}() {{
{content}
}}")
            Invoke($"Main{rndstr}")
        End Sub
        Public Sub Run(content As String)
            Execute(content)
        End Sub
        Public Function LoadModule(content As String) As ModuleInfo
            Dim rndstr As String = Guid.NewGuid.ToString.Replace("-", "_")
            Dim moduleInfo As New ModuleInfo
            'Try
            MyBase.Execute($"function Main{rndstr}(moduleInfo) {{
{content}
}}")
                Invoke($"Main{rndstr}", moduleInfo)
                'Catch ex As Exception
                '    APIs.API.LogErr("模块加载失败：" & ex.ToString)
                'End Try
                Return moduleInfo
        End Function
        Public Class ModuleInfo
            Public Property Author As String = "unknown"
            Public Property Description As String = "unknown"
            Public Property Version As String = "unknown"
        End Class
    End Class
    Public Property Engine As EngineEx = Nothing
    Private Function CreateJSEngine() As EngineEx
        Dim options As Options = New Options()
        options.AllowClr()
        options.AllowClr(GetType(Main).Assembly)
        'APIs.API.Log(Assembly.GetExecutingAssembly.GetName)
        'APIs.API.Log(Assembly.GetCallingAssembly.GetName)
        'APIs.API.Log(Assembly.GetEntryAssembly.GetName)
        options.AllowClr(GetType(FileIO.FileSystem).Assembly)
        options.AllowClr(GetType(Directory).Assembly)
        options.AllowClr(GetType(Process).Assembly)
        options.AllowClr(GetType(System.Threading.Thread).Assembly)
#If DEBUG Then
        options.DebugMode()
        options.CatchClrExceptions()
#End If
        options.AllowClr(AssemblyEx.AssemblyList.Values.ToArray())
        'System.Console.WriteLine(String.Join(vbCrLf, Assembly.GetCallingAssembly.GetExportedTypes.ToList().ConvertAll(Function(l) l.Assembly.FullName)))
        'For Each item In Assembly.GetExecutingAssembly().GetReferencedAssemblies()
        '    System.Console.WriteLine(item.Name)
        '    'If item.Name = "System.Runtime" Then
        '    '    options.AllowClr(item.CodeBase)
        '    'End If
        'Next

        'options.AllowClr(GetType(System.IO.Directory).Assembly)
        'options.CatchClrExceptions(Function(e)
        '                               Return False
        '                           End Function)
        'options.AllowClr(GetType(Newtonsoft.Json.JsonConvert).Assembly)
        'engine.SetValue("TheType", TypeReference.CreateTypeReference(engine, TypeOf (TheType)))
        Engine = New EngineEx(options)
#If DEBUG Then
        'Engine.SetValue("testfunc", Sub(obj)
        '                                Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(obj))
        '                            End Sub)
#End If
        'engine.SetValue("ResourceFiles", Runtime.Interop.TypeReference.CreateTypeReference(engine, GetType()))
        'engine.SetValue("ConnectionManager", Runtime.Interop.TypeReference.CreateTypeReference(engine, GetType(ConnectionManager)))
        'engine.SetValue("AssemblyEx", Runtime.Interop.TypeReference.CreateTypeReference(engine, GetType(AssemblyEx)))
        'engine.SetValue("FileSystem", Runtime.Interop.TypeReference.CreateTypeReference(engine, GetType(Microsoft.VisualBasic.FileIO.FileSystem)))
        'FileIO.FileSystem.GetFiles().First()
        'engine.SetValue("api", API)
        'engine.SetValue("MCConnections", MCConnections)
        'engine.SetValue("events", Events)
        'engine.SetValue("engine", engine)
        Return Engine
    End Function
    Private Function StartJSEngine(e As EngineEx, RestartDuration As UInteger) As EngineEx
        Try
            Try
                APIs.Events.QQ.OnGroupMessage.Clear()
            Catch : End Try
#If DEBUG Then
            e.Execute(JSRaw)
#Else
            e.Execute(JSRaw)
#End If
            'engine=Nothing
            MCConnections.ForEach(Sub(l) l.CheckConnect())
        Catch ex As ReloadEngineException
            StartJSEngine(CreateJSEngine, RestartDuration)
        Catch ex As Exception
            API.LogErr("JS引擎遇到错误:" & ex.ToString)
            Dim d As TimeSpan = TimeSpan.FromMilliseconds(RestartDuration)
            API.Log($"将在{If(d.Minutes > 0, d.Minutes & "分", "")}{d.Seconds}秒后重新加载！")
            Dim t As New Timer(RestartDuration)
            AddHandler t.Elapsed, Sub()
                                      StartJSEngine(CreateJSEngine, RestartDuration * 2)
                                      t.Dispose()
                                  End Sub
            t.Start()
        End Try
        Return e
    End Function

    Public Sub Init(_api As IBridgeQQBase)
        API = _api '保存API
        API.Log("正在加载PFBridgeCore...")
        StartJSEngine(CreateJSEngine, 1000)
    End Sub
    Friend ReadOnly Property JSRaw As String
        Get
#If DEBUG Then
            If Not Directory.Exists(Path.Combine(API.PluginDataPath, "scripts ")) Then Directory.CreateDirectory(Path.Combine(API.PluginDataPath, "scripts "))
            File.WriteAllText(Path.Combine(API.PluginDataPath, "scripts\main.js"), My.Resources.ResourceFiles.main)
            File.WriteAllText(Path.Combine(API.PluginDataPath, "scripts\query.js"), My.Resources.ResourceFiles.query)
            File.WriteAllText(Path.Combine(API.PluginDataPath, "scripts\command.js"), My.Resources.ResourceFiles.command)
            File.WriteAllText(Path.Combine(API.PluginDataPath, "scripts\whitelist.js"), My.Resources.ResourceFiles.whitelist)
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
