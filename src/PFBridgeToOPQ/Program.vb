Imports System
Imports Traceless

Module Program
    Async Function Main() As Task
        Await OPQSDK.Plugin.OPQMain.Client()
    End Function
End Module
