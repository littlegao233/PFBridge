Imports System.Reflection
Public Module AssemblyEx
    Public AssemblyList As New Dictionary(Of String, Assembly)
    Public Function LoadFrom(path As String) As Boolean
        Try
            If AssemblyList.ContainsKey(path) Then
                Return False
            End If

            AssemblyList.Add(path, Assembly.LoadFrom(path))
            API.Log("[AssemblyEx] [√] 程序集""" & IO.Path.GetFileName(path) & """加载成功！")
            Return True
        Catch ex As IO.FileLoadException
            API.LogErr("[AssemblyEx] [×] 程序集""" & IO.Path.GetFileName(path) & """加载失败！(已跳过)(" & ex.Message & ")")
            Return False
        Catch ex As IO.FileNotFoundException
            API.LogErr("[AssemblyEx] [×] 程序集""" & IO.Path.GetFileName(path) & """未找到！(已跳过)(" & ex.Message & ")")
            Return False
        End Try
    End Function
    'Public Sub LoadFile(path As String)
    '    AssemblyList.Add(Assembly.LoadFile(path))
    'End Sub
    Friend Class ReloadEngineException
        Inherits Exception
    End Class
    Public Sub ReloadEngine()
        Throw New ReloadEngineException
    End Sub
End Module
