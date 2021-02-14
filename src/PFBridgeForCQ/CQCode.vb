Imports PFBridgeCore
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Text.RegularExpressions

Public Module CodeEx
    Friend Function CQDeCode(ByVal source As String) As String
        If Equals(source, Nothing) Then Return String.Empty
        Return CQDeCode(New StringBuilder(source))
    End Function

    Friend Function CQDeCode(ByVal builder As StringBuilder) As String
        builder = builder.Replace("&#91;", "[")
        builder = builder.Replace("&#93;", "]")
        builder = builder.Replace("&#44;", ",")
        builder = builder.Replace("&amp;", "&")
        Return builder.ToString()
    End Function

    ''' <summary>
    ''' 获取字符串副本的转义形式
    ''' </summary>
    ''' <paramname="source">欲转义的原始字符串</param>
    ''' <paramname="enCodeComma">是否转义逗号, 默认 <code>false</code></param>
    ''' <exceptioncref="ArgumentNullException">参数: source 为 null</exception>
    ''' <returns>返回转义后的字符串副本</returns>
    Public Function CQEnCode(ByVal source As String, ByVal enCodeComma As Boolean) As String
        If Equals(source, Nothing) Then Return String.Empty
        Dim builder As StringBuilder = New StringBuilder(source)
        builder = builder.Replace("&", "&amp;")
        builder = builder.Replace("[", "&#91;")
        builder = builder.Replace("]", "&#93;")
        If enCodeComma Then builder = builder.Replace(",", "&#44;")
        Return builder.ToString()
    End Function

    Friend Function ParseMessage(ByVal raw As String, ByVal group As Long) As String
        Dim builder As StringBuilder = New StringBuilder(raw)

        For Each code In CQCode.Parse(raw)
            Select Case code.Function
                Case CQFunction.Face
                    builder.Replace(code.Original, APIs.API.ParseMessageFormat.Face)
                Case CQFunction.Image
                    builder.Replace(code.Original, APIs.API.ParseMessageFormat.Image)
                Case CQFunction.Record
                    builder.Replace(code.Original, APIs.API.ParseMessageFormat.Record)
                Case CQFunction.Video
                    builder.Replace(code.Original, APIs.API.ParseMessageFormat.Video)
                Case CQFunction.Music
                    Dim url As String
                    If Not code.Items.TryGetValue("url", url) Then url = "?"
                    Dim title As String
                    If Not code.Items.TryGetValue("title", title) Then title = "?"
                    builder.Replace(code.Original, String.Format(APIs.API.ParseMessageFormat.Music, url, title))
                    Exit Select
                Case CQFunction.At
                    Dim target As String = code.Items("qq")
                    If Equals(target, "all") Then
                        builder.Replace(code.Original, APIs.API.ParseMessageFormat.AtAll)
                    Else
                        builder.Replace(code.Original, String.Format(APIs.API.ParseMessageFormat.At, GetMemberCard(group, Long.Parse(target))))
                    End If
                Case CQFunction.Share
                    Dim url As String = "?"
                    If Not code.Items.TryGetValue("url", url) Then url = "?"
                    Dim title As String = "?"
                    If Not code.Items.TryGetValue("title", title) Then title = "?"
                    builder.Replace(code.Original, String.Format(APIs.API.ParseMessageFormat.Share, url, title))
                    Exit Select
                Case CQFunction.Reply
                    builder.Replace(code.Original, APIs.API.ParseMessageFormat.Reply)
                Case CQFunction.Forward
                    builder.Replace(code.Original, APIs.API.ParseMessageFormat.Forward)
                                        'case Sora.Enumeration.CQFunction.Poke:
                    '    builder.Replace(code.Original,APIs.API.ParseMessageFormat.Image); break;
                Case CQFunction.Rich
                    builder.Replace(code.Original, APIs.API.ParseMessageFormat.Json)
                                        'case Sora.Enumeration.CQFunction.RedBag: break;
                    'case Sora.Enumeration.CQFunction.Gift: break;
                Case CQFunction.CardImage
                    'case Sora.Enumeration.CQFunction.TTS: break;
                    builder.Replace(code.Original, APIs.API.ParseMessageFormat.Image)
                Case Else
                    builder.Replace(code.Original, String.Format(APIs.API.ParseMessageFormat.Unknown, code.UnknownType))
            End Select
        Next

        Return CQDeCode(builder)
    End Function
    Friend Function GetMemberCard(group As Long, qq As Long) As String
        Dim member = PFBridgeForCQ.GetMember(group, qq)
        Dim result As String = member.DisplayName
        If String.IsNullOrEmpty(result) Then result = member.Nickname
        Return result
    End Function
End Module

<DefaultValue(CQFunction.Unknown)>
Public Enum CQFunction
    <Description("unknown")> Unknown
    <Description("face")> Face
    <Description("emoji")> Emoji
    <Description("bface")> Bface
    <Description("sface")> Sface
    <Description("image")> Image
    <Description("record")> Record
    <Description("at")> At
    <Description("rps")> Rps
    <Description("dice")> Dice
    <Description("shake")> Shake
    <Description("music")> Music
    <Description("share")> Share
    <Description("rich")> Rich
    <Description("sign")> Sign
    <Description("hb")> Hb
    <Description("contact")> Contact
    <Description("show")> Show
    <Description("location")> Location
    <Description("anonymous")> Anonymous
    CardImage
    Reply
    Forward
    Video
End Enum
''' <summary>
''' 表示 CQ码 的类
''' </summary>
Public Class CQCode
#Region "--字段--"
    Private Shared ReadOnly _regices As Lazy(Of Regex()) = New Lazy(Of Regex())(AddressOf InitializeRegex)
    Private _originalString As String
    Private _type As CQFunction
    Friend UnknownType As String
    Private _items As Dictionary(Of String, String)

#End Region

#Region "--属性--"
    Public ReadOnly Property Original As String
        Get
            Return _originalString
        End Get
    End Property

    ''' <summary>
    ''' 获取一个值, 指示当前实例的功能
    ''' </summary>
    Public ReadOnly Property [Function] As CQFunction
        Get
            Return _type
        End Get
    End Property


    ''' <summary>
    ''' 获取当前实例所包含的所有项目
    ''' </summary>
    Public ReadOnly Property Items As Dictionary(Of String, String)
        Get
            Return _items
        End Get
    End Property


    ''' <summary>
    ''' 获取一个值, 指示当前实例是否属于图片 <seecref="CQCode"/>
    ''' </summary>
    Public ReadOnly Property IsImageCQCode As Boolean
        Get
            Return EqualIsImageCQCode(Me)
        End Get
    End Property


    ''' <summary>
    ''' 获取一个值, 指示当前实例是否属于语音 <seecref="CQCode"/>
    ''' </summary>
    Public ReadOnly Property IsRecordCQCode As Boolean
        Get
            Return EqualIsRecordCQCode(Me)
        End Get
    End Property

#End Region

#Region "--构造函数--"
    ''' <summary>
    ''' 使用 CQ码 字符串初始化 <seecref="CQCode"/> 类的新实例
    ''' </summary>
    ''' <paramname="str">CQ码字符串 或 包含CQ码的字符串</param>
    Private Sub New(ByVal str As String)
        _originalString = str

#Region "--解析 CqCode--"
        Dim match As Match = _regices.Value(0).Match(str)

        If Not match.Success Then
            Throw New FormatException("无法解析所传入的字符串, 字符串非CQ码格式!")
        End If

#End Region

#Region "--解析CQ码类型--"
        If Not [Enum].TryParse(Of CQFunction)(match.Groups(CInt(1)).Value, True, _type) Then
            _type = CQFunction.Unknown    ' 解析不出来的时候, 直接给一个默认
            UnknownType = match.Groups(CInt(1)).Value
        End If

#End Region

#Region "--解析键值对--"
        Dim collection As MatchCollection = _regices.Value(1).Matches(match.Groups(2).Value)
        _items = New Dictionary(Of String, String)(collection.Count)

        For Each item As Match In collection
            _items.Add(item.Groups(1).Value, CQDeCode(item.Groups(2).Value))
        Next
#End Region
    End Sub


    ''' <summary>
    ''' 初始化 <seecref="CQCode"/> 类的新实例
    ''' </summary>
    ''' <paramname="type">CQ码类型</param>
    ''' <paramname="keyValues">包含的键值对</param>
    Public Sub New(ByVal type As CQFunction, ParamArray keyValues As KeyValuePair(Of String, String)())
        _type = type
        _items = New Dictionary(Of String, String)(keyValues.Length)

        For Each item As KeyValuePair(Of String, String) In keyValues
            _items.Add(item.Key, item.Value)
        Next

        _originalString = Nothing
    End Sub

#End Region

#Region "--公开方法--"
    ''' <summary>
    ''' 从字符串中解析出所有的 CQ码, 转换为 <seecref="CQCode"/> 集合
    ''' </summary>
    ''' <paramname="source">原始字符串</param>
    ''' <returns>返回等效的 <seecref="List(OfCqCode)"/></returns>
    Public Shared Function Parse(ByVal source As String) As List(Of CQCode)
        Dim collection As MatchCollection = _regices.Value(0).Matches(source)
        Dim codes As List(Of CQCode) = New List(Of CQCode)(collection.Count)

        For Each item As Match In collection
            codes.Add(New CQCode(item.Groups(0).Value))
        Next

        Return codes
    End Function

    ''' <summary>
    ''' 判断是否是图片 <seecref="CQCode"/>
    ''' </summary>
    ''' <paramname="code">要判断的 <seecref="CQCode"/> 实例</param>
    ''' <returns>如果是图片 <seecref="CQCode"/> 返回 <seelangword="true"/> 否则返回 <seelangword="false"/></returns>
    Public Shared Function EqualIsImageCQCode(ByVal code As CQCode) As Boolean
        Return code.Function = CQFunction.Image
    End Function

    ''' <summary>
    ''' 判断是否是语音 <seecref="CQCode"/>
    ''' </summary>
    ''' <paramname="code">要判断的 <seecref="CQCode"/> 实例</param>
    ''' <returns>如果是语音 <seecref="CQCode"/> 返回 <seelangword="true"/> 否则返回 <seelangword="false"/></returns>
    Public Shared Function EqualIsRecordCQCode(ByVal code As CQCode) As Boolean
        Return code.Function = CQFunction.Record
    End Function

    ''' <summary>
    ''' 确定指定的对象是否等于当前对象
    ''' </summary>
    ''' <paramname="obj">要与当前对象进行比较的对象</param>
    ''' <returns>如果指定的对象等于当前对象，则为 <code>true</code>，否则为 <code>false</code></returns>	
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        Dim code As CQCode = TryCast(obj, CQCode)

        If code IsNot Nothing Then
            Return String.Equals(_originalString, code._originalString)
        End If

        Return MyBase.Equals(obj)
    End Function

    ''' <summary>
    ''' 返回该字符串的哈希代码
    ''' </summary>
    ''' <returns> 32 位有符号整数哈希代码</returns>
    Public Overrides Function GetHashCode() As Integer
        Return MyBase.GetHashCode() And _originalString.GetHashCode()
    End Function

    ''' <summary>
    ''' 返回此实例等效的CQ码形式
    ''' </summary>
    ''' <returns></returns>
    Public Overrides Function ToString() As String
        If Equals(_originalString, Nothing) Then

            If _items.Count = 0 Then
                ' 特殊CQ码, 抖动窗口
                _originalString = String.Format("[CQ:{0}]", _type.ToString())
            Else
                ' 普通CQ码, 带参数
                Dim builder As StringBuilder = New StringBuilder()
                builder.Append("[CQ:")
                builder.Append(_type.ToString())   ' function
                For Each item As KeyValuePair(Of String, String) In _items
                    builder.AppendFormat(",{0}={1}", item.Key, CQEnCode(item.Value, True))
                Next

                builder.Append("]")
                _originalString = builder.ToString()
            End If
        End If

        Return _originalString
    End Function

    ''' <summary>
    ''' 处理返回用于发送的字符串
    ''' </summary>
    ''' <returns>用于发送的字符串</returns>
    Public Function ToSendString() As String
        Return ToString()
    End Function

#End Region

#Region "--私有方法--"
    ''' <summary>
    ''' 延时初始化正则表达式
    ''' </summary>
    ''' <returns></returns>
    Private Shared Function InitializeRegex() As Regex()
        ' 此处延时加载, 以提升运行速度
        Return New Regex() {New Regex("\[CQ:([A-Za-z]*)(?:(,[^\[\]]+))?\]", RegexOptions.Compiled), New Regex(",([A-Za-z]+)=([^,\[\]]+)", RegexOptions.Compiled)}    ' 匹配CQ码
        ' 匹配键值对
    End Function
#End Region
End Class
