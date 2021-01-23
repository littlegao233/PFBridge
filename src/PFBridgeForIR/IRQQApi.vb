Imports System
Imports System.Runtime.InteropServices

Namespace IRQQ.CSharp
    Friend Module IRApi
        Public Sub LoginAllQQ()
            For Each qq In Api_GetOffLineList().Replace(vbCr, "").Split(New Char() {ChrW(10)}, options:=StringSplitOptions.RemoveEmptyEntries)
                Api_LoginQQ(qq)
            Next
        End Sub
        Public Function GetOnlineQQ() As String()
            'return IRQQApi.Api_GetQQList().Replace("\r","").Split('\n');
            Return Api_GetOnLineList().Replace(vbCr, "").Split(ChrW(10))
        End Function
        Public Sub InvokeAsAllQQ(ByVal act As Action(Of String))
            For Each robotQQ In GetOnlineQQ()
                act.Invoke(robotQQ)
            Next
        End Sub
        'public static string[] GetOnlineQQ()
        '{
        'return    IRQQApi.Api_GetQQList().Replace("\r","").Split('\n');
        '    //return new List<string>()
        '}
        Public Sub SendGroupMessage(ByVal group As Long, ByVal message As String)
            SendGroupMessage(group.ToString(), message)
        End Sub
        Public Sub SendGroupMessage(ByVal group As String, ByVal message As String)
            InvokeAsAllQQ(Sub(robotQQ) Api_SendMsg(robotQQ, 2, group.ToString(), "", message, -1))
        End Sub

        Public Function GetGroupName(ByVal robotQQ As String, ByVal group As String) As String
            Return Api_GetGroupName(robotQQ, group)
        End Function

        Public Function GetNickName(ByVal robotQQ As String, ByVal qq As String) As String
            Return Api_GetNick(robotQQ, qq)
        End Function

        Public Function GetMemberCard(ByVal robotQQ As String, ByVal group As String, ByVal qq As String) As String
            Return Api_GetGroupCard(robotQQ, group, qq)
        End Function

        Public Sub SendPrivateMessageFromGroup(ByVal group As Long, ByVal qq As Long, ByVal message As String)
            InvokeAsAllQQ(Sub(robotQQ) Api_SendMsg(robotQQ, 4, group.ToString(), qq.ToString(), message, -1))
        End Sub

        Public Sub Log(ByVal content As String)
            Api_OutPutLog(content)
        End Sub

        Public ReadOnly Property PluginDataPath As String
            Get
                Return Environment.CurrentDirectory & "\PFBridge"
            End Get
        End Property
    End Module

    Friend Module IRQQApiCore
        Friend Const IRApiPath As String = "../Bin/IRapi.dll" 'ApiPath
        'internal const string IRApiPath = "../Bin/ERapi.dll";//ApiPath
        <DllImport(IRApiPath, EntryPoint:="Api_GetAge")>
        Friend Function ApiGetAge(ByVal RobotQQ As String, ByVal ObjQQ As String) As Integer
        End Function

        ''' <summary>
        ''' 将好友拉入黑名单，成功返回真，失败返回假
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        ''' <param name="ObjQQ">欲拉黑的QQ</param>
        <DllImport(IRApiPath)>
        Friend Function Api_AddBkList(ByVal RobotQQ As String, ByVal ObjQQ As String) As Boolean
        End Function

        ''' <summary>
        ''' 向框架帐号列表增加一个登录QQ，成功返回真（CleverQQ可用）
        ''' </summary>
        ''' <param name="RobotQQ">帐号</param>
        ''' <param name="PassWord">密码</param>
        ''' <param name="Auto">自动登录</param>
        <DllImport(IRApiPath)>
        Friend Function Api_AddQQ(ByVal RobotQQ As String, ByVal PassWord As String, ByVal Auto As Boolean) As String
        End Function

        ''' <summary>
        ''' 管理员邀请对象入群，每次只能邀请一个对象，频率过快会失败
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        ''' <param name="ObjQQ">被邀请对象QQ</param>
        ''' <param name="GroupNum">欲邀请加入的群号</param>
        <DllImport(IRApiPath)>
        Friend Sub Api_AdminInviteGroup(ByVal RobotQQ As String, ByVal ObjQQ As String, ByVal GroupNum As String)
        End Sub

        ''' <summary>
        ''' 创建一个讨论组，成功返回讨论组ID，失败返回空
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        ''' <param name="DisGroupName">讨论组名称</param>
        <DllImport(IRApiPath)>
        Friend Function Api_CreateDisGroup(ByVal RobotQQ As String, ByVal DisGroupName As String) As String
        End Function

        ''' <summary><param name="RobotQQ">响应的QQ</param>
        ''' <param name="ObjQQ">对象QQ</param></summary>      
        <DllImport(IRApiPath)>
        Friend Sub Api_DelBkList(ByVal RobotQQ As String, ByVal ObjQQ As String)
        End Sub

        ''' <summary>
        ''' 删除好友，成功返回真，失败返回假
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        ''' <param name="ObjQQ">欲删除对象QQ</param>
        <DllImport(IRApiPath)>
        Friend Function Api_DelFriend(ByVal RobotQQ As String, ByVal ObjQQ As String) As Boolean
        End Function

        ''' <summary>
        ''' 请求禁用插件自身
        ''' </summary>
        <DllImport(IRApiPath)>
        Friend Sub Api_DisabledPlugin()
        End Sub

        ''' <summary>
        ''' 取得机器人网页操作用参数Bkn或G_tk
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        <DllImport(IRApiPath)>
        Friend Function Api_GetBkn(ByVal RobotQQ As String) As String
        End Function

        ''' <summary>
        ''' 取得机器人网页操作用参数长Bkn或长G_tk
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        <DllImport(IRApiPath)>
        Friend Function Api_GetBkn32(ByVal RobotQQ As String) As String
        End Function

        ''' <summary>
        ''' 取得腾讯微博页面操作用参数P_skey
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        <DllImport(IRApiPath)>
        Friend Function Api_GetBlogPsKey(ByVal RobotQQ As String) As String
        End Function

        ''' <summary>
        ''' 取得腾讯课堂页面操作用参数P_skye
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        <DllImport(IRApiPath)>
        Friend Function Api_GetClassRoomPsKey(ByVal RobotQQ As String) As String
        End Function

        ''' <summary>
        ''' 取得机器人网页操作用的Clientkey
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        <DllImport(IRApiPath)>
        Friend Function Api_GetClientkey(ByVal RobotQQ As String) As String
        End Function

        ''' <summary>
        ''' 取得机器人网页操作用的Cookies
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        <DllImport(IRApiPath)>
        Friend Function Api_GetCookies(ByVal RobotQQ As String) As String
        End Function

        ''' <summary>
        ''' 取得讨论组列表
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        <DllImport(IRApiPath)>
        Friend Function Api_GetDisGroupList(ByVal RobotQQ As String) As String
        End Function

        ''' <summary>
        ''' 取邮箱，当对象QQ不为10000@qq.com时，可用于获取正确邮箱
        ''' </summary>
        ''' <param name="RobotQQ">响应的QQ</param>
        ''' <param name="ObjQQ">对象QQ</param>
        <DllImport(IRApiPath)>
        Friend Function Api_GetEmail(ByVal RobotQQ As String, ByVal ObjQQ As String) As String
        End Function

        ''' <summary>
        ''' 取得好友列表，返回获取到的原始JSON格式信息，需自行解析
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        <DllImport(IRApiPath)>
        Friend Function Api_GetFriendList(ByVal RobotQQ As String) As String
        End Function

        ''' <summary>
        ''' 取对象性别 1男 2女 未知或失败返回-1
        ''' </summary>
        ''' <param name="RobotQQ">响应的QQ</param>
        ''' <param name="ObjQQ">对象QQ</param>
        <DllImport(IRApiPath)>
        Friend Function Api_GetGender(ByVal RobotQQ As String, ByVal ObjQQ As String) As Integer
        End Function

        ''' <summary>
        ''' 取得群管理员，返回获取到的原始JSON格式信息，需自行解析
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        ''' <param name="GroupNum">欲取管理员列表群号</param>
        <DllImport(IRApiPath)>
        Friend Function Api_GetGroupAdmin(ByVal RobotQQ As String, ByVal GroupNum As String) As String
        End Function

        ''' <summary>
        ''' 取对象群名片
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        ''' <param name="GroupNum">群号</param>
        ''' <param name="ObjQQ">欲取得群名片的QQ号码</param>
        <DllImport(IRApiPath)>
        Friend Function Api_GetGroupCard(ByVal RobotQQ As String, ByVal GroupNum As String, ByVal ObjQQ As String) As String
        End Function

        ''' <summary>
        ''' 取得群列表，返回获取到的原始JSON格式信息，需自行解析
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        <DllImport(IRApiPath)>
        Friend Function Api_GetGroupList(ByVal RobotQQ As String) As String
        End Function

        ''' <summary>
        ''' 取得群成员列表，返回获取到的原始JSON格式信息，需自行解析
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        ''' <param name="GroupNum">欲取群成员列表群号</param>
        <DllImport(IRApiPath)>
        Friend Function Api_GetGroupMemberList(ByVal RobotQQ As String, ByVal GroupNum As String) As String
        End Function

        ''' <summary>
        ''' 取QQ群名
        ''' </summary>
        ''' <param name="RobotQQ">响应的QQ</param>
        ''' <param name="GroupNum">群号</param>
        <DllImport(IRApiPath)>
        Friend Function Api_GetGroupName(ByVal RobotQQ As String, ByVal GroupNum As String) As String
        End Function

        ''' <summary>
        ''' 取得QQ群页面操作用参数P_skye
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        <DllImport(IRApiPath)>
        Friend Function Api_GetGroupPsKey(ByVal RobotQQ As String) As String
        End Function

        ''' <summary>
        ''' 取框架日志
        ''' </summary>
        <DllImport(IRApiPath)>
        Friend Function Api_GetLog() As String
        End Function

        ''' <summary>
        ''' 取得机器人操作网页用的长Clientkey
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        <DllImport(IRApiPath)>
        Friend Function Api_GetLongClientkey(ByVal RobotQQ As String) As String
        End Function

        ''' <summary>
        ''' 取得机器人操作网页用参数长Ldw
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        ''' <returns></returns>
        <DllImport(IRApiPath)>
        Friend Function Api_GetLongLdw(ByVal RobotQQ As String) As String
        End Function

        ''' <summary>
        ''' 取对象昵称
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        ''' <param name="ObjQQ">欲取得的QQ号码</param>
        <DllImport(IRApiPath)>
        Friend Function Api_GetNick(ByVal RobotQQ As String, ByVal ObjQQ As String) As String
        End Function

        ''' <summary>
        ''' 取群公告，返回该群所有公告，JSON格式，需自行解析
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        ''' <param name="GroupNum">欲取得公告的群号</param>
        <DllImport(IRApiPath)>
        Friend Function Api_GetNotice(ByVal RobotQQ As String, ByVal GroupNum As String) As String
        End Function

        ''' <summary>
        ''' 获取对象资料，此方式为http，调用时应自行注意控制频率（成功返回JSON格式需自行解析）
        ''' </summary>
        ''' <param name="RobotQQ">响应的QQ</param>
        ''' <param name="ObjQQ">对象QQ</param>
        <DllImport(IRApiPath)>
        Friend Function Api_GetObjInfo(ByVal RobotQQ As String, ByVal ObjQQ As String) As String
        End Function

        ''' <summary>
        ''' 取对象QQ等级，成功返回等级，失败返回-1
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        ''' <param name="ObjQQ">欲取得的QQ号码</param>
        <DllImport(IRApiPath)>
        Friend Function Api_GetObjLevel(ByVal RobotQQ As String, ByVal ObjQQ As String) As Integer
        End Function

        ''' <summary>
        ''' 获取对象当前赞数量，石板返回-1，成功返回赞数量（获取频繁会出现失败现象，请自行写判断处理失败问题）
        ''' </summary>
        ''' <param name="RobotQQ">响应的QQ</param>
        ''' <param name="ObjQQ">对象QQ</param>
        <DllImport(IRApiPath)>
        Friend Function Api_GetObjVote(ByVal RobotQQ As String, ByVal ObjQQ As String) As Long
        End Function

        ''' <summary>
        ''' 取框架离线QQ号（多Q版可用）
        ''' </summary>
        <DllImport(IRApiPath)>
        Friend Function Api_GetOffLineList() As String
        End Function

        ''' <summary>
        ''' 取框架在线QQ号（多Q版可用）
        ''' </summary>
        <DllImport(IRApiPath)>
        Friend Function Api_GetOnLineList() As String
        End Function

        ''' <summary>
        ''' 取个人说明
        ''' </summary>
        ''' <param name="RobotQQ">响应的QQ</param>
        ''' <param name="ObjQQ">对象QQ</param>
        <DllImport(IRApiPath)>
        Friend Function Api_GetPerExp(ByVal RobotQQ As String, ByVal ObjQQ As String) As String
        End Function

        ''' <summary>
        ''' 根据图片GUID取得图片下载链接
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        ''' <param name="PicType">图片类型</param>
        ''' <param name="ReferenceObj">参考对象</param>
        ''' <param name="PicGUID">图片GUID</param>
        <DllImport(IRApiPath)>
        Friend Function Api_GetPicLink(ByVal RobotQQ As String, ByVal PicType As Integer, ByVal ReferenceObj As String, ByVal PicGUID As String) As String
        End Function

        ''' <summary>
        ''' 取Q龄，成功返回Q龄，失败返回-1
        ''' </summary>
        ''' <param name="RobotQQ">响应的QQ</param>
        ''' <param name="ObjQQ">对象QQ</param>
        <DllImport(IRApiPath)>
        Friend Function Api_GetQQAge(ByVal RobotQQ As String, ByVal ObjQQ As String) As Integer
        End Function

        ''' <summary>
        ''' 取框架所有QQ号
        ''' </summary>
        <DllImport(IRApiPath)>
        Friend Function Api_GetQQList() As String
        End Function

        ''' <summary>
        ''' 获取机器人状态信息，成功返回：昵称、账号、在线状态、速度、收信、发信、在线时间，失败返回空
        ''' </summary>
        ''' <param name="RobotQQ">响应的QQ</param>
        <DllImport(IRApiPath)>
        Friend Function Api_GetRInf(ByVal RobotQQ As String) As String
        End Function

        ''' <summary>
        ''' 取个性签名
        ''' </summary>
        ''' <param name="RobotQQ"></param>
        ''' <param name="ObjQQ">对象QQ</param>
        ''' <returns></returns>
        <DllImport(IRApiPath)>
        Friend Function Api_GetSign(ByVal RobotQQ As String, ByVal ObjQQ As String) As String
        End Function

        ''' <summary>
        ''' 获取当前框架内部时间戳
        ''' </summary>
        <DllImport(IRApiPath)>
        Friend Function Api_GetTimeStamp() As Long
        End Function

        ''' <summary>
        ''' 取框架版本号
        ''' </summary>
        <DllImport(IRApiPath)>
        Friend Function Api_GetVer() As String
        End Function

        ''' <summary>
        ''' 取得QQ空间页面操作有用参数P_skye
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        <DllImport(IRApiPath)>
        Friend Function Api_GetZonePsKey(ByVal RobotQQ As String) As String
        End Function

        ''' <summary>
        ''' 群ID转群号
        ''' </summary>
        ''' <param name="GroupID">群ID</param>
        <DllImport(IRApiPath)>
        Friend Function Api_GIDTransGN(ByVal GroupID As String) As String
        End Function

        ''' <summary>
        ''' 群号转群ID
        ''' </summary>
        ''' <param name="GroupNum">群号</param>
        <DllImport(IRApiPath)>
        Friend Function Api_GNTransGID(ByVal GroupNum As String) As String
        End Function

        ''' <summary>
        ''' 处理框架所有事件请求
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        ''' <param name="ReQuestType">请求类型：213请求入群，214我被邀请加入某群，215某人被邀请加入群，101某人请求添加好友</param>
        ''' <param name="ObjQQ">对象QQ：申请入群，被邀请人，请求添加好友人的QQ（当请求类型为214时这里请为空）</param>
        ''' <param name="GroupNum">群号：收到请求的群号（好友添加时留空）</param>
        ''' <param name="Handling">处理方式：10同意 20拒绝 30忽略</param>
        ''' <param name="AdditionalInfo">附加信息：拒绝入群附加信息</param>
        <DllImport(IRApiPath)>
        Friend Sub Api_HandleEvent(ByVal RobotQQ As String, ByVal ReQuestType As Integer, ByVal ObjQQ As String, ByVal GroupNum As String, ByVal Handling As Integer, ByVal AdditionalInfo As String)
        End Sub

        ''' <summary>
        ''' 是否QQ好友，好友返回真，非好友或获取失败返回假
        ''' </summary>
        ''' <param name="RobotQQ">响应的QQ</param>
        ''' <param name="OBjQQ">对象QQ</param>
        <DllImport(IRApiPath)>
        Friend Function Api_IfFriend(ByVal RobotQQ As String, ByVal ObjQQ As String) As Boolean
        End Function

        ''' <summary>
        ''' 邀请对象加入讨论组，成功返回空，失败返回理由
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        ''' <param name="DisGroupID"></param>
        ''' <param name="ObjQQ">被邀请对象QQ：多个用 换行符 分割</param>
        ''' <returns></returns>
        <DllImport(IRApiPath)>
        Friend Function Api_InviteDisGroup(ByVal RobotQQ As String, ByVal DisGroupID As String, ByVal ObjQQ As String) As String
        End Function

        ''' <summary>
        ''' 取得插件自身启用状态，启用真，禁用假
        ''' </summary>
        <DllImport(IRApiPath)>
        Friend Function Api_IsEnable() As Boolean
        End Function


        ''' <summary>
        ''' 查询对象或自己是否被禁言，禁言返回真，失败或未禁言返回假
        ''' </summary>
        ''' <param name="RobotQQ">响应的QQ</param>
        ''' <param name="GroupNum">群号</param>
        ''' <param name="ObjQQ">对象QQ</param>
        <DllImport(IRApiPath)>
        Friend Function Api_IsShutUp(ByVal RobotQQ As String, ByVal GroupNum As String, ByVal ObjQQ As String) As Boolean
        End Function


        ''' <summary>
        ''' 申请加群
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        ''' <param name="GroupNum">群号</param>
        ''' <param name="Reason">附加理由，可留空</param>
        <DllImport(IRApiPath)>
        Friend Sub Api_JoinGroup(ByVal RobotQQ As String, ByVal GroupNUm As String, ByVal Reason As String)
        End Sub

        ''' <summary>
        ''' 将对象移除讨论组，成功返回空，失败返回理由
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        ''' <param name="DisGroupID">需要执行的讨论组ID</param>
        ''' <param name="ObjQQ">被执行对象</param>
        <DllImport(IRApiPath)>
        Friend Function Api_KickDisGroupMBR(ByVal RobotQQ As String, ByVal DisGroupID As String, ByVal ObjQQ As String) As String
        End Function


        ''' <summary>
        ''' 将对象移出群
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        ''' <param name="GroupNum">群号</param>
        ''' <param name="ObjQQ">被执行对象</param>
        <DllImport(IRApiPath)>
        Friend Sub Api_KickGroupMBR(ByVal RobotQQ As String, ByVal GroupNum As String, ByVal ObjQQ As String)
        End Sub


        ''' <summary>
        ''' 载入插件
        ''' </summary>
        <DllImport(IRApiPath)>
        Friend Sub Api_LoadPlugin()
        End Sub

        ''' <summary>
        ''' 登录指定QQ，应确保QQ号码在列表中已经存在
        ''' </summary>
        ''' <param name="RobotQQ">欲登录的QQ</param>
        <DllImport(IRApiPath)>
        Friend Sub Api_LoginQQ(ByVal RobotQQ As String)
        End Sub

        ''' <summary>
        ''' 非管理员邀请对象入群，每次只能邀请一个对象，频率过快会失败
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        ''' <param name="ObjQQ">被邀请人QQ号码</param>
        ''' <param name="GroupNum">群号</param>
        <DllImport(IRApiPath)>
        Friend Sub Api_NoAdminInviteGroup(ByVal RobotQQ As String, ByVal ObjQQ As String, ByVal GroupNum As String)
        End Sub

        ''' <summary>
        ''' 令指定QQ下线，应确保QQ号码已在列表中且在线
        ''' </summary>
        ''' <param name="RobotQQ">欲下线的QQ</param>
        <DllImport(IRApiPath)>
        Friend Sub Api_OffLineQQ(ByVal RobotQQ As String)
        End Sub

        ''' <summary>
        ''' 向IRQQ日志窗口发送一条本插件的日志，可用于调试输出需要的内容，或定位插件错误与运行状态
        ''' </summary>
        ''' <param name="Log">日志信息</param>
        <DllImport(IRApiPath)>
        Friend Sub Api_OutPutLog(ByVal Log As String)
        End Sub

        ''' <summary>
        ''' 发布群公告（成功返回真，失败返回假），调用此API应保证响应QQ为管理员
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        ''' <param name="GroupNum">欲发布公告的群号</param>
        ''' <param name="Title">公告标题</param>
        ''' <param name="Content">内容</param>
        <DllImport(IRApiPath)>
        Friend Function Api_PBGroupNotic(ByVal RobotQQ As String, ByVal GroupNum As String, ByVal Title As String, ByVal Content As String) As Boolean
        End Function

        ''' <summary>
        ''' 发布QQ群作业
        ''' </summary>
        ''' <param name="RobotQQ">响应的QQ</param>
        ''' <param name="GroupNum">群号</param>
        ''' <param name="HomeWorkName">作业名</param>
        ''' <param name="HomeWorkTitle">作业标题</param>
        ''' <param name="Text">作业内容</param>
        <DllImport(IRApiPath)>
        Friend Function Api_PBHomeWork(ByVal RobotQQ As String, ByVal GroupNum As String, ByVal HomeWorkName As String, ByVal HomeWorkTitle As String, ByVal Text As String) As String
        End Function

        ''' <summary>
        ''' 发送QQ说说
        ''' </summary>
        ''' <param name="RobotQQ">响应的QQ</param>
        ''' <param name="Text">发送内容</param>
        <DllImport(IRApiPath)>
        Friend Function Api_PBTaoTao(ByVal RobotQQ As String, ByVal Text As String) As String
        End Function

        ''' <summary>
        ''' 退出讨论组
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        ''' <param name="DisGroupID">需要退出的讨论组ID</param>
        <DllImport(IRApiPath)>
        Friend Sub Api_QuitDisGroup(ByVal RobotQQ As String, ByVal DisGroupID As String)
        End Sub

        ''' <summary>
        ''' 退群
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        ''' <param name="GroupNum">欲退出的群号</param>
        <DllImport(IRApiPath)>
        Friend Sub Api_QuitGroup(ByVal RobotQQ As String, ByVal GroupNum As String)
        End Sub

        ''' <summary>
        ''' 发送JSON结构消息
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        ''' <param name="SendType">发送方式：1普通 2匿名（匿名需要群开启）</param>
        ''' <param name="MsgType">信息类型：1好友 2群 3讨论组 4群临时会话 5讨论组临时会话</param>
        ''' <param name="MsgTo">收信对象所属群_讨论组（消息来源），发送群、讨论组、临时会话填写、如发送对象为好友可留空</param>
        ''' <param name="ObjQQ">收信对象QQ</param>
        ''' <param name="Json">Json结构内容</param>
        <DllImport(IRApiPath)>
        Friend Sub Api_SendJSON(ByVal RobotQQ As String, ByVal SendType As Integer, ByVal MsgType As Integer, ByVal MsgTo As String, ByVal ObjQQ As String, ByVal Json As String)
        End Sub



        ''' <summary>
        ''' 发送普通文本消息
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        ''' <param name="MsgType">信息类型：1好友 2群 3讨论组 4群临时会话 5讨论组临时会话</param>
        ''' <param name="MsgTo">收信对象群_讨论组：发送群、讨论组、临时会话时填写</param>
        ''' <param name="ObjQQ">收信QQ</param>
        ''' <param name="Msg">内容</param>
        ''' <param name="ABID">气泡ID</param>
        <DllImport(IRApiPath)>
        Friend Sub Api_SendMsg(ByVal RobotQQ As String, ByVal MsgType As Integer, ByVal MsgTo As String, ByVal ObjQQ As String, ByVal Msg As String, ByVal ABID As Integer)
        End Sub

        ''' <summary>
        ''' 向腾讯发送原始封包（成功返回腾讯返回的包）
        ''' </summary>
        ''' <param name="PcakText">封包内容</param>
        <DllImport(IRApiPath)>
        Friend Function Api_SendPack(ByVal PcakText As String) As String
        End Function

        ''' <summary>
        ''' 好友语音上传并发送（成功返回真，失败返回假） 
        ''' </summary>      
        ''' <param name="RobotQQ">响应的QQ</param>
        ''' <param name="ObjQQ">接收QQ</param>
        ''' <param name="pAmr">语音数据的指针</param>
        <DllImport(IRApiPath)>
        Friend Function Api_SendVoice(ByVal RobotQQ As String, ByVal ObjQQ As String, ByVal pAmr As Integer) As Boolean
        End Function
        ''' <summary>
        ''' 发送XML消息
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        ''' <param name="SendType">发送方式：1普通 2匿名（匿名需要群开启）</param>
        ''' <param name="MsgFrom"></param>
        ''' <param name="MsgTo">收信对象群、讨论组：发送群、讨论组、临时时填写，如MsgType为好友可空</param>
        ''' <param name="ObjQQ">收信对象QQ</param>
        ''' <param name="ObjectMsg">XML代码</param>
        ''' <param name="ObjCType">结构子类型：00基本 02点歌</param>
        <DllImport(IRApiPath)>
        Friend Sub Api_SendXML(ByVal RobotQQ As String, ByVal SendType As Integer, ByVal MsgFrom As Integer, ByVal MsgTo As String, ByVal ObjQQ As String, ByVal ObjectMsg As String, ByVal ObjCType As Integer)
        End Sub

        ''' <summary>
        ''' 获取会话SessionKey密匙
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        <DllImport(IRApiPath)>
        Friend Function Api_SessionKey(ByVal RobotQQ As String) As String
        End Function


        ''' <summary>
        ''' 设置或取消管理员，成功返回空，失败返回理由
        ''' </summary>
        ''' <param name="RobotQQ">响应的QQ</param>
        ''' <param name="GroupNum">群号</param>
        ''' <param name="ObjQQ">对象QQ</param>
        ''' <param name="SetWay">操作方式，真为设置管理，假为取消管理</param>
        <DllImport(IRApiPath)>
        Friend Function Api_SetAdmin(ByVal RobotQQ As String, ByVal GroupNum As String, ByVal ObjQQ As String, ByVal SetWay As Boolean) As String
        End Function


        ''' <summary>
        ''' 开关群匿名消息发送功能，成功返回真，失败返回假
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        ''' <param name="GroupNum">群号</param>
        ''' <param name="Swit">开关：真开 假关</param>
        <DllImport(IRApiPath)>
        Friend Function Api_SetAnon(ByVal RobotQQ As String, ByVal GroupNum As String, ByVal swit As Boolean) As Boolean
        End Function


        ''' <summary>
        ''' 修改对象群名片，成功返回真，失败返回假
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        ''' <param name="GroupNum">群号</param>
        ''' <param name="ObjQQ">对象QQ：被修改名片人QQ</param>
        ''' <param name="NewCard">需要修改的群名片</param>
        <DllImport(IRApiPath)>
        Friend Function Api_SetGroupCard(ByVal RobotQQ As String, ByVal GroupNum As String, ByVal ObjQQ As String, ByVal NewCard As String) As Boolean
        End Function


        ''' <summary>
        ''' 修改机器人在线状态，昵称，个性签名等
        ''' </summary>
        ''' <param name="RobotQQ">响应的QQ</param>
        ''' <param name="type">1 我在线上 2 Q我吧 3 离开 4 忙碌 5 请勿打扰 6 隐身 7 修改昵称 8 修改个性签名</param>
        ''' <param name="ChangeText">修改内容，类型7和8时填写，其他为""</param>
        <DllImport(IRApiPath)>
        Friend Sub Api_SetRInf(ByVal RobotQQ As String, ByVal type As Integer, ByVal ChangeText As String)
        End Sub


        ''' <summary>
        ''' 屏蔽或接收某群消息
        ''' </summary>
        ''' <param name="RobotQQ">响应的QQ</param>
        ''' <param name="GroupNum">群号</param>
        ''' <param name="type">真为屏蔽接收，假为接收拼不提醒</param>
        <DllImport(IRApiPath)>
        Friend Sub Api_SetShieldedGroup(ByVal RobotQQ As String, ByVal GroupNum As String, ByVal type As Boolean)
        End Sub


        ''' <summary>
        ''' 向好友发起窗口抖动，调用此Api腾讯会限制频率
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        ''' <param name="ObjQQ">接收抖动消息的QQ</param>
        <DllImport(IRApiPath)>
        Friend Function Api_ShakeWindow(ByVal RobotQQ As String, ByVal ObjQQ As String) As Boolean
        End Function


        ''' <summary>
        ''' 禁言群内某人
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        ''' <param name="GroupNum">欲操作的群号</param>
        ''' <param name="ObjQQ">欲禁言对象，如留空且机器人QQ为管理员，将设置该群为全群禁言</param>
        ''' <param name="Time">禁言时间：0解除（秒），如为全群禁言，参数为非0，解除全群禁言为0</param>
        <DllImport(IRApiPath)>
        Friend Sub Api_ShutUP(ByVal RobotQQ As String, ByVal GroupNum As String, ByVal ObjQQ As String, ByVal Time As Integer)
        End Sub


        ''' <summary>
        ''' QQ群签到，成功返回真失败返回假
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        ''' <param name="GroupNum">群号</param>
        <DllImport(IRApiPath)>
        Friend Function Api_SignIn(ByVal RobotQQ As String, ByVal GroupNum As String) As Boolean
        End Function


        ''' <summary>
        ''' 腾讯Tea加密
        ''' </summary>
        ''' <param name="Text">需要加密的内容</param>
        ''' <param name="SessionKey">会话Key，从Api_SessionKey获得</param>
        <DllImport(IRApiPath)>
        Friend Function Api_Tea加密(ByVal Text As String, ByVal SessionKey As String) As String
        End Function


        ''' <summary>
        ''' 腾讯Tea解密
        ''' </summary>
        ''' <param name="Text">需解密的内容</param>
        ''' <param name="SessionKey">会话Key，从Api_SessionKey获得</param>
        <DllImport(IRApiPath)>
        Friend Function Api_Tea解密(ByVal Text As String, ByVal SessionKey As String) As String
        End Function


        ''' <summary>
        ''' 卸载插件自身
        ''' </summary>
        <DllImport(IRApiPath)>
        Friend Sub Api_UninstallPlugin()
        End Sub

        ''' <summary>
        ''' 上传图片（通过读入字节集方式），可使用网页链接或本地读入，成功返回该图片GUID,失败返回空
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        ''' <param name="UpLoadType">上传类型：1好友2群PS:好友临时用1，群讨论组用2；当填写错误时，图片GUID发送不会成功</param>
        ''' <param name="UpTo">参考对象，上传该图片所属群号或QQ</param>
        ''' <param name="Pic">图片字节集数据</param>
        ''' <returns></returns>
        <DllImport(IRApiPath)>
        Friend Function Api_UpLoadPic(ByVal RobotQQ As String, ByVal UpLoadType As Integer, ByVal UpTo As String, ByVal Pic As Integer) As String
        End Function


        ''' <summary>
        ''' 上传QQ语音，成功返回语音GUID，失败返回空
        ''' </summary>
        ''' <param name="RobotQQ">响应的QQ</param>
        ''' <param name="type">上传类型 2 QQ群</param>
        ''' <param name="GroupNum">接收的群号</param>
        ''' <param name="pAmr">语音数据指针</param>
        <DllImport(IRApiPath)>
        Friend Function Api_UpLoadVoice(ByVal RobotQQ As String, ByVal type As Integer, ByVal GroupNum As String, ByVal pAmr As Integer) As String
        End Function


        ''' <summary>
        ''' 调用一次点一下，成功返回空，失败返回理由如：每天最多给他点十个赞哦，调用此Api时应注意频率，每人每日10次，至多50人
        ''' </summary>
        ''' <param name="RobotQQ">机器人QQ</param>
        ''' <param name="ObjQQ">被赞人QQ</param>
        <DllImport(IRApiPath)>
        Friend Function Api_UpVote(ByVal RobotQQ As String, ByVal ObjQQ As String) As String
        End Function
    End Module
End Namespace
