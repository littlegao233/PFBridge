Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Namespace PFWebsocketAPI.Model
    <JsonConverter(GetType(Converters.StringEnumConverter))>
    Public Enum PackType '基本包类型
        pack
        encrypted
    End Enum
    <JsonConverter(GetType(Converters.StringEnumConverter))>
    Public Enum EncryptionMode '加密模式
        AES256
    End Enum
    Friend MustInherit Class PackBase '基础类
        <JsonProperty(Order:=-3)>
        Public MustOverride ReadOnly Property type As PackType '包类型
        Public Overrides Function ToString() As String
            Return JsonConvert.SerializeObject(Me) '基类的转化为String重写方法
        End Function
        Public Function GetParams(Of T)(json As JObject) As T
            Return json("params").ToObject(Of T) '基类的获取参数表方法
        End Function
    End Class
    Friend Class EncryptedPack    '加密包
        Inherits PackBase
        Public Overrides ReadOnly Property type As PackType = PackType.encrypted
        Public params As ParamMap
        Friend Sub New(json As JObject) '通过已有json初始化对象（通常用作传入解析）
            params = GetParams(Of ParamMap)(json) '通过基类该方法获取参数表
        End Sub
        Friend Sub New(mode As EncryptionMode, from As String, password As String) '通过参数初始化包（通常用作发送前）
            params = New ParamMap With {.mode = mode, .raw = SimpleAES.AES256.Encrypt(from, password)}
        End Sub
        Public Function Decode(password As String) As String '解密params.raw中的内容并返回
            Dim decoded = SimpleAES.AES256.Decrypt(params.raw, password) 'AES256解密
            If String.IsNullOrEmpty(decoded) Then Throw New Exception("AES256 Decode failed!")
            Return decoded
        End Function
        Friend Class ParamMap '对象参数表
            Public mode As EncryptionMode
            Public raw As String
        End Class
    End Class
    Friend Class OriginalPack   '普通包/解密后的包
        Inherits PackBase
        Public Overrides ReadOnly Property type As PackType = PackType.pack
    End Class

#Region "Server"
    <JsonConverter(GetType(Converters.StringEnumConverter))>
    Public Enum ServerCauseType
        chat
        join
        left
        cmd
        runcmdfeedback
        decodefailed
        invalidrequest
    End Enum
    Friend MustInherit Class ServerPackBase
        Inherits OriginalPack
        '<JsonProperty("cause")>
        <JsonProperty(Order:=-2)>
        Public MustOverride ReadOnly Property cause As ServerCauseType
    End Class
    Friend Class CauseJoin
        Inherits ServerPackBase
        Friend Sub New(json As JObject)
            params = GetParams(Of ParamMap)(json)
        End Sub
        Friend Sub New(sender As String, xuid As String, uuid As String, ip As String)
            params = New ParamMap With {.sender = sender, .xuid = xuid, .uuid = uuid, .ip = ip}
        End Sub
        Public Overrides ReadOnly Property cause As ServerCauseType = ServerCauseType.join
        Public params As ParamMap
        Friend Class ParamMap
            Public sender, xuid, uuid, ip As String
        End Class
    End Class
    Friend Class CauseLeft
        Inherits ServerPackBase
        Friend Sub New(json As JObject)
            params = GetParams(Of ParamMap)(json)
        End Sub
        Friend Sub New(sender As String, xuid As String, uuid As String, ip As String)
            params = New ParamMap With {.sender = sender, .xuid = xuid, .uuid = uuid, .ip = ip}
        End Sub
        Public Overrides ReadOnly Property cause As ServerCauseType = ServerCauseType.left
        Public params As ParamMap
        Friend Class ParamMap
            Public sender, xuid, uuid, ip As String
        End Class
    End Class
    Friend Class CauseChat
        Inherits ServerPackBase
        Friend Sub New(json As JObject)
            params = GetParams(Of ParamMap)(json)
        End Sub
        Friend Sub New(sender As String, text As String)
            params = New ParamMap With {.sender = sender, .text = text}
        End Sub
        Public Overrides ReadOnly Property cause As ServerCauseType = ServerCauseType.chat
        Public params As ParamMap
        Friend Class ParamMap
            Public sender, text As String
        End Class
    End Class
    Friend Class CauseCmd
        Inherits ServerPackBase
        Friend Sub New(json As JObject)
            params = GetParams(Of ParamMap)(json)
        End Sub
        Friend Sub New(sender As String, text As String)
            params = New ParamMap With {.sender = sender, .text = text}
        End Sub
        Public Overrides ReadOnly Property cause As ServerCauseType = ServerCauseType.cmd
        Public params As ParamMap
        Friend Class ParamMap
            Public sender, text As String
        End Class
    End Class
    '命令返回
    Friend Class CauseRuncmdFeedback
        Inherits ServerPackBase
        Friend Sub New(json As JObject)
            params = GetParams(Of ParamMap)(json)
        End Sub
        Friend Sub New(id As String, cmd As String, result As String, con As Object)
            params = New ParamMap With {.id = id, .cmd = cmd, .result = result, .con = con}
        End Sub
        Public Overrides ReadOnly Property cause As ServerCauseType = ServerCauseType.runcmdfeedback
        Public params As ParamMap
        Friend Class ParamMap
            Public id As String
            Public result As String
            <JsonIgnore>
            Friend cmd As String
            <JsonIgnore>
            Friend waiting As Integer = 0
            <JsonIgnore>
            Friend con As Object
        End Class
    End Class
    Friend Class CauseDecodeFailed
        Inherits ServerPackBase
        Friend Sub New(json As JObject)
            params = GetParams(Of ParamMap)(json)
        End Sub
        Friend Sub New(msg As String)
            params = New ParamMap With {.msg = msg}
        End Sub
        Public Overrides ReadOnly Property cause As ServerCauseType = ServerCauseType.decodefailed
        Public params As ParamMap
        Friend Class ParamMap
            Public msg As String
        End Class
    End Class
    Friend Class CauseInvalidRequest
        Inherits ServerPackBase
        Friend Sub New(json As JObject)
            params = GetParams(Of ParamMap)(json)
        End Sub
        Friend Sub New(msg As String)
            params = New ParamMap With {.msg = msg}
        End Sub
        Public Overrides ReadOnly Property cause As ServerCauseType = ServerCauseType.invalidrequest
        Public params As ParamMap
        Friend Class ParamMap
            Public msg As String
        End Class
    End Class

#End Region
#Region "Client"
    <JsonConverter(GetType(Converters.StringEnumConverter))>
    Public Enum ClientActionType
        runcmdrequest
        broadcast
        tellraw
    End Enum
    Friend MustInherit Class ClientPackBase
        Inherits OriginalPack
        <JsonProperty(Order:=-2)>
        Public MustOverride ReadOnly Property action As ClientActionType
    End Class
    Friend Class ActionRunCmd
        Inherits ClientPackBase
        'Friend Sub New(json As JObject)
        '    params = GetParams(Of ParamMap)(json)
        'End Sub
        Friend Sub New(json As JObject, con As Object)
            params = GetParams(Of ParamMap)(json)
            params.con = con
        End Sub
        Friend Sub New(cmd As String, id As String, con As Object)
            params = New ParamMap With {.cmd = cmd, .id = id, .con = con}
        End Sub
        Public Overrides ReadOnly Property action As ClientActionType = ClientActionType.runcmdrequest
        Public params As ParamMap
        Friend Class ParamMap
            Public cmd, id As String
            <JsonIgnore>
            Friend con As Object
        End Class
        Friend Function GetFeedback() As CauseRuncmdFeedback
            Return New CauseRuncmdFeedback(params.id, params.cmd, Nothing, params.con)
        End Function
    End Class
#End Region
End Namespace
