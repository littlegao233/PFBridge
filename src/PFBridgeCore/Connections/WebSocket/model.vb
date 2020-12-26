Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Namespace PFWebsocketAPI
    'Public Enum ServerMessageType
    '    onmsg
    '    oncmd
    '    onjoin
    '    onleft
    'End Enum
    'Friend Class ServerMessage
    '    Friend Property TypeCode As ServerMessageType
    '    Friend Class EventArgsConverter
    '        Inherits JsonConverter
    '        Public Overrides Sub WriteJson(writer As JsonWriter, value As Object, serializer As JsonSerializer)
    '            writer.WriteValue(value.ToString)
    '        End Sub
    '        Public Overrides Function ReadJson(reader As JsonReader, objectType As Type, existingValue As Object, serializer As JsonSerializer) As Object
    '            Select Case Type
    '                Case 
    '            End Select

    '            Return OnMsgEventArgs.GetFrom(reader.Value.ToString)
    '        End Function
    '        Public Overrides Function CanConvert(ByVal objectType As Type) As Boolean
    '            Return True
    '        End Function
    '    End Class
    '    <JsonConverter(GetType(EventArgsConverter))>
    '    Friend Property Args As BaseArgs
    '    Public Overrides Function ToString() As String
    '        Return JsonConvert.SerializeObject(Me)
    '    End Function
    'End Class
    'Friend MustInherit Class BaseArgs
    '    Public Overrides Function ToString() As String
    '        Return JsonConvert.SerializeObject(Me)
    '    End Function
    'End Class
    'Friend Class OnMsgEventArgs
    '    Inherits BaseArgs
    '    Public Shared Shadows Function GetFrom(raw As String) As OnMsgEventArgs
    '        Return JsonConvert.DeserializeObject(Of OnMsgEventArgs)(raw)
    '    End Function
    'End Class


End Namespace
