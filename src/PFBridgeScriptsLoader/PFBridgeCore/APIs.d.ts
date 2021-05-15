/// <reference path="./System.d.ts" />
declare interface PFBridgeCoreAPIs {
    /**
     * 社交系统的简洁API
     */
    API: IBridgeIMBase
    /**
     * 事件系统
     */
    Events: IBridgeEventsMap
}
declare interface IBridgeIMBase {
    /**
     * 获取插件数据目录(只读属性)
     */
    PluginDataPath: string
    /**
     * 群聊消息的转换格式
     * 用于 群聊天=>服务器 的富文本消息转换
     */
    ParseMessageFormat: IParseMessageFormat
    /**
     * 输出框架日志信息
     * @param Message 信息
     */
    Log(Message: any): void
    /**
     * 输出框架错误日志信息
     * @param Message 信息
     */
    LogErr(Message: any): void
    /**
     * 发送群消息
     * @param TargetGroup 目标群号
     * @param Message 消息内容
     */
    SendGroupMessage(TargetGroup: number, Message: string): void
    /**
     * 通过某个群向指定QQ号发送临时会话（私聊消息）
     * @param TargetGroup 群号
     * @param QQid 目标QQ
     * @param Message 消息内容
     */
    SendPrivateMessageFromGroup(TargetGroup: number, QQid: number, Message: string): void
}


declare interface IParseMessageFormat {
    At: string
    AtAll: string
    Image: string
    Emoji: string
    Face: string
    Bface: string
    Record: string
    Video: string
    Share: string
    Music: string
    Reply: string
    Forward: string
    Node: string
    Xml: string
    Json: string
    File: string
    Unknown: string
}
declare interface IBridgeEventsMap {
    /**
     * IM聊天事件
     * （如：QQ信息触发的事件）
     */
    IM: IBridgeIMEventsMap
    /**
     * MC远程事件
     * （如：mc反馈的玩家事件）
     */
    MC: IBridgeMCEventsMap
}
declare interface IEventsMapItem<T> {
    /**
     * 添加事件回调函数
     * @param callback 回调函数
     */
    Add(callback: ((eventArgs: T) => void)): void
    /**
     * 清除所有事件回调函数
     */
    Clear(): void
}
declare interface IBridgeIMEventsMap {
    /**
     * 群消息回调
     */
    OnGroupMessage: IEventsMapItem<BridgeIMEventsOnGroupMessage>
}
declare class BridgeIMEventsOnGroupMessage {
    /**
     * 消息来源群号码
     */
    public get groupId(): number
    /**
     * 消息来源群名称
     */
    public get groupName(): string
    /**
     * 消息发送者ID（QQ号）
     */
    public get senderId(): number
    /**
     * 消息发送者昵称
     */
    public get senderNick(): string
    /**
     * 消息发送者群名片
     */
    public get memberCard(): string
    /**
     * 消息发送者(群成员)类型
     */
    public get memberType(): memberTypeEnum | number
    public get message(): string
    public get messageMatch(): MessageMatchCmd
    public get parsedMessage(): string
    /*Func<string> getGroupName { get; }
    Func<string> getQQNick { get; }
    Func<string> getQQCard { get; }
    Func<int> getMemberType { get; }
    Action<string> feedback { get; }
    Func<string> getParsedMessage { get; }*/
    /**
     * 回复此消息
     * @param message 消息内容
     */
    feedback(message: string): void

}
declare const enum memberTypeEnum {
    /**
     * 未知
     */
    Unknown = 0,
    /**
     * 成员
     */
    Member = 1,
    /**
     * 管理员
     */
    Admin = 2,
    /**
     * 群主
     */
    Owner = 3
}
declare interface MessageMatchCmd {
    getCommands(...start: (string | string[])[]): System.Collections.Generic.List<string>
}
declare interface IBridgeMCEventsMap {
    /**
     * 玩家加入事件回调
     */
    Join: IEventsMapItem<BridgeMCPlayerJoinEventsArgs>;
    /**
     * 玩家离开事件回调
     */
    Left: IEventsMapItem<BridgeMCPlayerLeftEventsArgs>;
    /**
     * 玩家聊天消息回调
     */
    Chat: IEventsMapItem<BridgeMCPlayerChatEventsArgs>;
    /**
     * 玩家输入命令回调
     */
    Cmd: IEventsMapItem<BridgeMCPlayerCmdEventsArgs>;
    /**
     * （命名）生物死亡回调
     */
    MobDie: IEventsMapItem<BridgeMCMobDieEventsArgs>;
}
declare class BridgeMCBaseEventsArgs {
    /** 当前MC连接实例 */
    public connection: IBridgeMCBase;
}
declare class BridgeMCPlayerJoinEventsArgs extends BridgeMCBaseEventsArgs {
    /** 加入玩家的ID */
    public get sender(): string
    /** 加入玩家的IP */
    public get ip(): string
    /** 加入玩家的uuid标识 */
    public get uuid(): string
    /** 加入玩家的xuid */
    public get xuid(): string
}

declare class BridgeMCPlayerLeftEventsArgs extends BridgeMCBaseEventsArgs {
    /** 退出玩家的ID */
    public get sender(): string
    /** 退出玩家的IP */
    public get ip(): string
    /** 退出玩家的uuid标识 */
    public get uuid(): string
    /** 退出玩家的xuid */
    public get xuid(): string
}
declare class BridgeMCPlayerChatEventsArgs extends BridgeMCBaseEventsArgs {
    /** 发送者ID */
    public get sender(): string
    /** 发送的聊天消息 */
    public get message(): string
}
declare class BridgeMCPlayerCmdEventsArgs extends BridgeMCBaseEventsArgs {
    /** 发送者ID */
    public get sender(): string
    /** 发送的命令内容 */
    public get cmd(): string
}
declare class BridgeMCMobDieEventsArgs extends BridgeMCBaseEventsArgs {
    /** 死亡生物的名称 */
    public get mobname(): string
    /** 死亡生物的类型 */
    public get mobtype(): string
    /** 死亡原因 */
    public get dmcase(): number
    /** 伤害源（攻击者）名称 */
    public get srcname(): string
    /** 伤害源（攻击者）类型 */
    public get srctype(): string
    /** 死亡坐标 */
    public get pos(): PFBridgeCore.PFWebsocketAPI.Model.Vec3
}
declare namespace PFBridgeCore.PFWebsocketAPI.Model {
    class Vec3 {
        /** x坐标 */
        public x: number
        /** y坐标 */
        public y: number
        /** z坐标 */
        public z: number
    }
}
declare interface IBridgeMCBase {
    Id: number
    Tag: any
    /**
     * 当前MC连接实例状态
     * @see true : 连接正常
     * @see false : 离线
     */
    State: boolean
    /**
     * 远程执行命令
     * @param cmd 命令内容
     * @param callback 命令执行结果回调
     */
    RunCmdCallback(cmd: string, callback: ((result: string) => void)): void
    /**
     * 远程执行命令
     * @param cmd 命令内容
     */
    RunCmd(cmd: string): void
    CheckConnect(): void
}