moduleInfo.Author = "littlegao233"
moduleInfo.Version = "v0.0.2"
moduleInfo.Description = "包含配置文件的读写、\n服务器之间的同步、\n群与服务器的聊天同步、\n加入服务器的群反馈"

let AdminQQs = new Array()
let Groups = new Array()
let Servers = new Array()
//#region 共享数据
const engine = importNamespace('PFBridgeCore').Main.Engine
engine.SetShareData("GetConfigGroups", () => { return Groups; })
engine.SetShareData("GetConfigAdminQQs", () => { return AdminQQs; })
//#endregion

//#region >>>>>-----公共方法(建议折叠)----->>>>>
const ConnectionManager = importNamespace('PFBridgeCore').ConnectionManager;
const events = importNamespace('PFBridgeCore').APIs.Events
const api = importNamespace('PFBridgeCore').APIs.API
const MCConnections = importNamespace('PFBridgeCore').ConnectionList.MCConnections

//保存文件
const File = importNamespace('System.IO').File;//导入命名空间
const configPath = api.pluginDataPath + "\\config.json"
function LoadConfig() {
    const JSONLinq = importNamespace('Newtonsoft.Json.Linq');//导入命名空间
    //用于读取带注释的json
    const imported = JSON.parse(JSONLinq.JObject.Parse(File.ReadAllText(configPath)).ToString())
    AdminQQs = imported.AdminQQs
    Groups = imported.Groups
    Servers = imported.Servers
}
if (File.Exists(configPath)) {
    LoadConfig();
    api.Log("从" + configPath + "读取配置文件成功")
} else {//输出默认配置文件
    const willexport = "{\n    \"AdminQQs\": [441870948, 233333]/*管理员QQ号,用于配置是否可执行命令等*/,\n    \"Groups\": [\n        {\n            \"id\": 626872357,//QQ群号\n            \"ServerMsgToGroup\": true,//是否转发服务器的各种消息到该群\n            \"GroupMsgToServer\": true//是否将该群的消息转发到所有服务器\n        }\n    ],\n    \"Servers\": [\n        {\n            \"type\": \"websocket\",\n            \"url\": \"ws://127.0.0.1:29132/mcws\",//websocket地址|如{\"Port\": \"29132\",\"EndPoint\": \"mcws\",\"Password\": \"commandpassword\"}对应ws://127.0.0.1:29132/mcws\n            \"token\": \"commandpassword\",//websocket密匙串（用于运行命令等操作）|\"Password\": \"commandpassword\"\n            \"name\": \"测试服务器\",\n            \"ServerMsgToGroup\": true,//是否将该服务器的各种消息转发到群\n            \"GroupMsgToServer\": true,//是否转发群消息到该服务器\n            \"ServerMsgToOther\": true,//是否将该服务器的各种消息转发到其他已连接服服务器（多服联动）\n            \"ReceiveMsgFromOther\": true,//是否接受其他服务器的消息（多服联动）\n            \"WhitelistEnabled\": true//是否开启白名单，改参数主要在whitelist.js中用到\n        }/*, {//在这里添加多个服务器\n            \"type\": \"websocket\",\n            \"url\": \"ws://127.0.0.1:29132/mcws\",//websocket地址\n            \"token\": \"commandpassword\",//websocket密匙串（用于运行命令等操作）\n        }*/\n    ]\n}"
    File.WriteAllText(configPath, willexport)
    api.Log("已输出默认配置文件到" + configPath)
    LoadConfig();
}
/**
 * 添加基于WebsocketAPI的mc连接
 * @param {string} url websocket地址
 * 格式：ws://地址:端口/终端
 * 参考：ws://127.0.0.1:29132/mcws
 * @param {string} token 密匙串（用于运行命令等操作）
 */
function AddWebsocket(url, token, tag) {
    return ConnectionManager.AddWebsocketClient(url, token, tag)
}
/**
 * 发送消息到所有已经连接并且配置开启GroupMsgToServer的MC服务器
 * @param {string} message 消息内容
 */
function SendBoardcastToAllServer(message) {
    MCConnections.forEach(connection => {
        let index = Servers.findIndex(s => s.id === connection.Id)
        if (index !== -1) {
            let server = Servers[index]
            if (server.GroupMsgToServer) {
                SendToServer(connection, message)
            }
        }
    })
}
/**
 * 发送消息到指定服务器
 * @param {*} connection 连接实例
 * @param {string} message 消息内容
 */
function SendToServer(connection, message) {
    connection.RunCmd(`tellraw @a {"rawtext":[{"text":"${encodeUnicode(message)}"}]}`)
}
/**
 * string转为unicode编码
 * @param {string} str 内容
 */
function encodeUnicode(str) {
    //return escape(str).replace(/%u/g, "\\u")
    var res = [];
    for (var i = 0; i < str.length; i++) { res[i] = ("00" + str.charCodeAt(i).toString(16)).slice(-4); }
    return "\\u" + res.join("\\u");
}
//#endregion <<<<<-----公共方法(建议折叠)-----<<<<<

//#region 服务器主体部分
Servers.forEach(server => {//添加所有服务器到实例
    if (server.type = "websocket") {
        server.id = AddWebsocket(server.url, server.token, server)
    } else {
        api.LogErr("未知的mc连接方案:" + server.type)
    }
});
events.Server.Chat.add(function (e) {
    const { connection, sender, message } = e
    const { Id } = connection
    let index = Servers.findIndex(s => s.id === Id);//匹配服务器（于配置中）
    if (index !== -1) {
        let server = Servers[index];
        if (server.ServerMsgToGroup) {
            ProcessServerMsgToGroup(`[${server.name}:Chat]${sender}>${message}`);
        }
        if (server.ServerMsgToOther) {
            ProcessServerMsgToOtherServer(Id, `§b【${server.name}消息】§e<${sender}>§a${message}`);
        }
    }
})
//events.Server.Cmd.add(function (e) {
//        const { connection, sender, cmd } = e
//        const { Id } = connection
//        ProcessServerMsgToGroup(JSON.stringify(e));
//})
events.Server.Join.add(function (e) {
    const { connection, sender, ip, uuid, xuid } = e
    const { Id } = connection
    let index = Servers.findIndex(s => s.id === Id);//匹配服务器（于配置中）
    if (index !== -1) {
        let server = Servers[index];
        if (server.ServerMsgToGroup) {
            ProcessServerMsgToGroup(`[${server.name}:Join]${sender}加入了服务器`);
        }
        if (server.ServerMsgToOther) {
            ProcessServerMsgToOtherServer(Id, `§b【${server.name}:Join】§e${sender}§a加入了服务器`);
        }
    }
})
events.Server.Left.add(function (e) {
    const { connection, sender, ip, uuid, xuid } = e
    const { Id } = connection
    let index = Servers.findIndex(s => s.id === Id);//匹配服务器（于配置中）
    if (index !== -1) {
        let server = Servers[index];
        if (server.ServerMsgToGroup) {
            ProcessServerMsgToGroup(`[${server.name}:Left]${sender}离开了服务器`);
        }
        if (server.ServerMsgToOther) {
            ProcessServerMsgToOtherServer(Id, `§b【${server.name}:Left】§e${sender}§a离开了服务器`);
        }
    }
})
function ProcessServerMsgToGroup(message) {
    Groups.forEach(group => {
        if (group.ServerMsgToGroup) {
            api.SendGroupMessage(group.id, message)
        }
    });
}
function ProcessServerMsgToOtherServer(id, message) {
    MCConnections.forEach(connection => {
        if (connection.Id !== id) {
            let index = Servers.findIndex(s => s.id === connection.Id)
            if (index !== -1) {
                let server = Servers[index]
                if (server.ReceiveMsgFromOther) {
                    SendToServer(connection, `${message}`)
                }
            }
        }
    });
}
//#endregion

//#region QQ主体部分
events.QQ.onGroupMessage.add(function (e) {
    const { groupId } = e
    let index = Groups.findIndex(l => l.id == groupId);//匹配群号（于配置）
    if (index !== -1) {
        let group = Groups[index];
        const {/* senderId,*/ message } = e
        if (message.startsWith('/') || message.startsWith('+')) {
        } else {
            if (group.GroupMsgToServer) {
                const { groupName,/* senderNick, */memberCard, parsedMessage } = e
                let msg = `§b【${groupName}】§e<${memberCard}>§a${parsedMessage}`
                SendBoardcastToAllServer(msg);
            }
            //api.SendPrivateMessageFromGroup(e.fromGroup, e.fromQQ, "test:" + e.message)
            //api.SendGroupMessage(e.fromGroup, "test1:" + e.message)

        }
    }
})
//#endregion