﻿Imports System.IO
Imports System.Reflection
Imports Esprima
Imports Jint
Imports Newtonsoft.Json.Linq
Imports Timer = System.Timers.Timer

Public Module Main
    Private Function CreateJSEngine() As Engine
        Dim options As Options = New Options()
        options.AllowClr()
        options.AllowClr(GetType(FileIO.FileSystem).Assembly)
        options.AllowClr(GetType(System.IO.Directory).Assembly)
        'options.CatchClrExceptions(Function(e)
        '                               Return False
        '                           End Function)
        'options.AllowClr(GetType(Newtonsoft.Json.JsonConvert).Assembly)
        'engine.SetValue("TheType", TypeReference.CreateTypeReference(engine, TypeOf (TheType)))
        Dim engine = New Engine(options)
        engine.SetValue("ResourceFiles", Runtime.Interop.TypeReference.CreateTypeReference(engine, GetType(My.Resources.ResourceFiles)))
        engine.SetValue("ConnectionManager", Runtime.Interop.TypeReference.CreateTypeReference(engine, GetType(ConnectionManager)))
        'engine.SetValue("FileSystem", Runtime.Interop.TypeReference.CreateTypeReference(engine, GetType(Microsoft.VisualBasic.FileIO.FileSystem)))
        'FileIO.FileSystem.GetFiles().First()
        engine.SetValue("api", API)
        engine.SetValue("events", Events)
        engine.SetValue("engine", engine)
        Return engine
    End Function
    Private Function StartJSEngine(e As Engine, RestartDuration As UInteger) As Engine
        Try
#If DEBUG Then
            e.Execute(JSRaw, New ParserOptions(JSRaw))
#Else
            e.Execute(JSRaw)
#End If
            'engine=Nothing
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
