﻿moduleInfo.Author = "littlegao233"
moduleInfo.Version = "v0.0.1"
moduleInfo.Description = '群内使用""/cmd [服务器] <命令>""命令执行服务器命令'
//管理员QQ请在main.js配置
const events = importNamespace('PFBridgeCore').APIs.Events
const api = importNamespace('PFBridgeCore').APIs.API
const MCConnections = importNamespace('PFBridgeCore').ConnectionList.MCConnections
events.QQ.onGroupMessage.add(function (e) {
    const { groupId } = e
    let index = ConfigGroups.findIndex(l => l.id == groupId);//匹配群号（于配置）
    if (index !== -1) {
        //let group = ConfigGroups[index];
        let msg = e.message;
        const { senderId, message } = e
        let cmds = e.messageMatch.getCommands("/", "+")//使用现有的匹配方法
        for (var i = 0; i < cmds.Length; i++) {
            api.log(cmds[i]);
        }
        //if (message.startsWith('/') || message.startsWith('+')) {//判断消息前缀
        //    const act1 = /^(\S+)/.exec(msg.substr(1))[0];
        //    switch (act1.toLowerCase()) {
        //        case "cmd": case "命令": 
        //            //if (AdminQQs.indexOf(e.fromQQ) === -1) {
        //            //    api.SendGroupMessage(e.fromGroup, "无权限!")
        //            //} else {

        //            //    })
        //            //}
        //            MCConnections.forEach(eachCon => {
        //                const ServerName = eachCon.Tag.name;
        //                eachCon.RunCmd("list", function (result) {
        //                    e.feedback(ServerName + "查询结果:\n" + result.trim())
        //                });
        //            });
        //        default:
        //    }
        //}
    }
})
