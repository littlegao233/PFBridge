//#region 服务器主体部分
AddWebsocket("ws://127.0.0.1:29132/mcws", "commandpassword")
const AdminQQs = new Array(441870948, 233333)

events.Server.Chat.add(function (e) {
    try {
        api.log(JSON.stringify(e));
    } catch (e) {
        api.LogErr("events.Server.Chat.error:" + e);
    }
})
events.Server.Cmd.add(function (e) {
    try {
        api.log(JSON.stringify(e));
    } catch (e) {
        api.LogErr("events.Server.Cmd.error:" + e);
    }
})
events.Server.Join.add(function (e) {
    try {
        api.log(JSON.stringify(e));
    } catch (e) {
        api.LogErr("events.Server.Join.error:" + e);
    }
})
events.Server.Left.add(function (e) {
    try {
        api.log(JSON.stringify(e));
    } catch (e) {
        api.LogErr("events.Server.Left.error:" + e);
    }
})
//#endregion

//#region QQ主体部分
events.QQ.onGroupMessage.add(function (e) {
    try {
        let msg = ""; msg = e.message;
        if (msg.startsWith('/')) {
            const act1 = /^(\S+)/.exec(msg.substr(1));
            api.log(act1[0]);
            if (AdminQQs.indexOf(e.fromQQ) === -1) {
                api.SendGroupMessage(e.fromGroup, "无权限!")
            } else { 
                MCConnections.forEach(l => {
                    l.RunCmd("say " + msg, function (l) { api.log("cb:"+l)})
                })
            }
        } else {
            api.log(JSON.stringify(e));
            SendBoardcastToAllServer(e.message);
            //api.SendPrivateMessageFromGroup(e.fromGroup, e.fromQQ, "test:" + e.message)
            //api.SendGroupMessage(e.fromGroup, "test1:" + e.message)
        }
    } catch (e) {
        api.LogErr("events.QQ.onGroupMessage.error:" + e);
    }
})
//#endregion
//#region 公共方法
/**
 * 发送消息到所有已经连接的MC服务器
 * @param {string} message 消息内容
 */
function SendBoardcastToAllServer(message) {
    MCConnections.forEach(l => {
        l.RunCmd("say " + message)
    })
}
//#endregion