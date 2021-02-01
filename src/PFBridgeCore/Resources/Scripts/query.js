moduleInfo.Author = "littlegao233"
moduleInfo.Version = "v0.0.1"
moduleInfo.Description = "群内使用/list命令查询服务器在线玩家\n服务器内使用/list命令自动反馈其他服务器的在线状态"


const MCConnections = importNamespace('PFBridgeCore').ConnectionList.MCConnections
const api = importNamespace('PFBridgeCore').APIs.API
//#region >>>>>-----公共方法(建议折叠)----->>>>>
// /**
// * 发送消息到所有已经连接并且配置开启GroupMsgToServer的MC服务器
// * @param {string} message 消息内容
// */
//function SendBoardcastToAllServer(message) {
//    MCConnections.forEach(connection => {
//        let index = Servers.findIndex(s => s.id === connection.Id)
//        if (index !== -1) {
//            let server = Servers[index]
//            if (server.GroupMsgToServer) {
//                SendToServer(connection, message)
//            }
//        }
//    })
//}
/**
 * 发送消息到指定玩家
 * @param {*} connection 连接实例
 * @param {string} playername 玩家名
 * @param {string} message 消息内容
 */
function SendToPlayer(connection, playername, message) {
    connection.RunCmd(`tellraw @a[name="${playername}"] {"rawtext":[{"text":"${encodeUnicode(message)}"}]}`)
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
const events = importNamespace('PFBridgeCore').APIs.Events
events.Server.Cmd.add(function (e) {
    const { connection, sender, cmd } = e;
    const { Id } = connection;
    if (cmd.trim().replace(/^\//, "").toLowerCase() == "list") {
        MCConnections.forEach(eachCon => {
            if (eachCon.Id !== Id) {
                const ServerName = eachCon.Tag.name;
                eachCon.RunCmd("list", result => {
                    SendToPlayer(connection, sender, `[${ServerName}在线状态]${result.trim().replace(/\n/g, "")}`);
                })
            }
        });
    }
    //const { Id } = connection
    //ProcessServerMsgToGroup(JSON.stringify(e));
})
events.QQ.onGroupMessage.add(function (e) {
    const { groupId } = e
    let index = ConfigGroups.findIndex(l => l.id == groupId);//匹配群号（于配置）
    if (index !== -1) {
        //let group = ConfigGroups[index];
        let msg = e.message;
        const { senderId, message } = e
        if (message.startsWith('/') || message.startsWith('+')) {//判断消息前缀
            let cmds = e.messageMatch.getCommands("/", "+")//使用现成的匹配方法
            if (cmds.Count >= 1) {
                switch (cmds[0]) {
                    case "list": case "查询": case "查服": case "query":
                        MCConnections.forEach(eachCon => {
                            const ServerName = eachCon.Tag.name;
                            eachCon.RunCmd("list", function (result) {
                                e.feedback(ServerName + "查询结果:\n" + result.trim())
                            });
                        });
                }
            } 
        }
    }
})
