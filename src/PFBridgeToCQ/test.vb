Imports System

Namespace PFBridgeToCQ
    Friend Class test
        Public Sub test1()
            Dim pool = New JsPool(New JsPoolConfig With {
                    ' This initializer will be called whenever a new engine is created. In a 
                    ' real app you'd commonly use ExecuteFile and ExecuteResource to load
                    ' libraries into the engine.
                    .Initializer = Sub(initEngine) initEngine.Execute("
      function sayHello(name) {
        return 'Hello ' + name + '!';
      }
    ")
            })

            ' Get an engine from the pool. The engine will be returned to the pool when
            ' disposed. In this case, the using() block will automatically return the
            ' engine to the pool when it falls out of scope.
            Using engine = pool.GetEngine()
                Dim message = engine.CallFunction(Of String)("sayHello", "Daniel")
                Console.WriteLine(message) ' "Hello Daniel!"
            End Using

            ' Disposing the pool will also dispose all its engines. Always dispose the pool
            ' when it is no longer required.
            pool.Dispose()
        End Sub
    End Class
End Namespace
