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
        Public Sub Run(content As String)
            Execute($"
function Main() {{
    {content}
}}
    ")
            Invoke("Main")
        End Sub
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

#If DEBUG Then
        options.AllowDebuggerStatement()
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
            e.Execute(JSRaw, New ParserOptions(JSRaw))
#Else
            e.Execute(JSRaw)
#End If
            'engine=Nothing
        Catch ex As AssemblyEx.ReloadEngineException
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
            File.WriteAllText(Path.Combine(API.PluginDataPath, "scripts\ListEx.js"), My.Resources.ResourceFiles.ListEx)
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
