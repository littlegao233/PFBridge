moduleInfo.Author = "littlegao233"
moduleInfo.Version = "v0.0.2"
moduleInfo.Description = "群内使用/list命令查询服务器在线玩家\n服务器内使用/list命令自动反馈其他服务器的在线状态"


var MCConnections = importNamespace('PFBridgeCore').ConnectionList.MCConnections
var api = importNamespace('PFBridgeCore').APIs.API
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
var SendToPlayer = function (connection: IBridgeMCBase, playername: string, message: string) {
    connection.RunCmd(`tellraw @a[name="${playername}"] {"rawtext":[{"text":"${encodeUnicode(message)}"}]}`)
}

/**
 * string转为unicode编码
 * @param {string} str 内容
 */
var encodeUnicode=function (str: string) {
    //return escape(str).replace(/%u/g, "\\u")
    var res = [];
    for (var i = 0; i < str.length; i++) { res[i] = ("00" + str.charCodeAt(i).toString(16)).slice(-4); }
    return "\\u" + res.join("\\u");
}

var Engine = importNamespace('PFBridgeCore').Main.Engine
var Data_GetConfigGroups = Engine.GetShareData("GetConfigGroups")
var Data_GetConfigAdminQQs = Engine.GetShareData("GetConfigAdminQQs")
var GetConfigGroups=function () { return Data_GetConfigGroups.Value(); }
var GetConfigAdminQQs=function () { return Data_GetConfigAdminQQs.Value(); }

//#endregion <<<<<-----公共方法(建议折叠)-----<<<<<
var events = importNamespace('PFBridgeCore').APIs.Events
events.MC.Cmd.Add(function (e) {
    const { connection, sender, cmd } = e;
    const { Id } = connection;
    if (cmd.trim().replace(/^\//, "").toLowerCase() == "list") {
        MCConnections.forEach(eachCon => {
            if (eachCon.Id !== Id) {
                const ServerName = eachCon.Tag.name;
                eachCon.RunCmdCallback("list", (result: string) => {
                    SendToPlayer(connection, sender, `[${ServerName}在线状态]${result.trim().replace(/\n/g, "")}`);
                })
            }
        });
    }
    //const { Id } = connection
    //ProcessServerMsgToGroup(JSON.stringify(e));
})
events.IM.OnGroupMessage.Add(function (e) {
    const { groupId } = e
    let index = GetConfigGroups().findIndex((l: { id: number }) => l.id == groupId);//匹配群号（于配置）
    if (index !== -1) {
        //let group = GetConfigGroups()[index];
        //let msg = e.message;
        const { /*senderId,*/ message } = e
        if (message.startsWith('/') || message.startsWith('+')) {//判断消息前缀
            let cmds = e.messageMatch.getCommands("/", "+")//使用现成的匹配方法
            if (cmds.Count >= 1) {
                switch (cmds[0]) {
                    case "list": case "查询": case "查服": case "query":
                        if (cmds.Count < 2) {//只有1个参数,如：/查询
                            let AllResult = new Array();
                            let TotalCount = MCConnections.length;
                            let SuccessCount = 0;
                            let FailedCount = 0;
                            for (let i = 0; i < TotalCount; i++) {
                                let thisCon = MCConnections[i]
                                const ServerName = thisCon.Tag.name;
                                if (thisCon.State) {
                                    AllResult.push([ServerName, "查询结果未知"])
                                    thisCon.RunCmdCallback('list', function (result: string) {
                                        try {
                                            let item = AllResult.find(x => x[0] == ServerName);
                                            item[1] = result.trim();
                                            SuccessCount++;
                                            if (TotalCount == SuccessCount + FailedCount && SuccessCount != 0) {//判断是否是执行的最后一个服务器
                                                let outputResult = [`在线查询结果:`];
                                                AllResult.forEach(x => outputResult.push(`[${x[0]}]${x[1].replace(/\n/g,"")}`));
                                                e.feedback(outputResult.join("\n"));
                                            }
                                        } catch (error) { }
                                    });
                                } else {
                                    FailedCount++;
                                    AllResult.push([ServerName, "服务器离线"])
                                }
                            }
                            if (FailedCount == TotalCount || TotalCount === 0) {
                                e.feedback("未查询到任何服务器");
                            }
                        } else if (cmds.Count < 3) {//只有1个参数,如：/查询 生存服
                            let NotEcecuted = true
                            MCConnections.forEach(eachCon => {
                                const ServerName = eachCon.Tag.name;
                                if (ServerName === cmds[1]) {
                                    eachCon.RunCmdCallback("list", function (result) {
                                        e.feedback(ServerName + "查询结果:\n" + result.trim())
                                    });
                                    NotEcecuted = false;
                                }
                            });
                            if (NotEcecuted) {
                                e.feedback(`未匹配到服务器：${cmds[1]}`)
                            }
                        }
                }
            }
        }
    }
})
