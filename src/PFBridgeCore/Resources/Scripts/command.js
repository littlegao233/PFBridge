moduleInfo.Author = "littlegao233";
moduleInfo.Version = "v0.0.2";
moduleInfo.Description = '群内使用"/cmd [服务器] <命令>"命令执行服务器命令';
//简单设置：
const JudgePermissionByConfig = true;
//JudgePermissionByConfig：是否通过配置文件判断权限
//  - true：仅有main.js配置的管理员QQ课执行
//  - false：发送者为群内的管理员就能执行
const events = importNamespace('PFBridgeCore').APIs.Events
const api = importNamespace('PFBridgeCore').APIs.API
const MCConnections = importNamespace('PFBridgeCore').ConnectionList.MCConnections
function JudgePermission(e) {
    if (JudgePermissionByConfig) {
        const { senderId } = e;
        //根据配置文件main.js中的管理员判断权限
        return GetConfigAdminQQs().some(l => l == senderId);
    } else {
        //根据成员类型判断权限（memberType属性=>0未知;1成员;2管理员;3群主）
        return e.memberType >= 2
    }
}
const Engine = importNamespace('PFBridgeCore').Main.Engine
const Data_GetConfigGroups = Engine.GetShareData("GetConfigGroups")
const Data_GetConfigAdminQQs = Engine.GetShareData("GetConfigAdminQQs")
function GetConfigGroups() { return Data_GetConfigGroups.Value();}
function GetConfigAdminQQs() { return Data_GetConfigAdminQQs.Value();}

events.IM.onGroupMessage.add(function (e) {
    const { groupId } = e
    let index = GetConfigGroups().findIndex(l => l.id == groupId);//匹配群号（于配置）
    if (index !== -1) {
        //let group = GetConfigGroups()[index];
        //let msg = e.message;
        const {  message } = e
        if (message.startsWith('/') || message.startsWith('+')) {//判断消息前缀
            let cmds = e.messageMatch.getCommands("/", "+")//使用现成的匹配方法
            if (cmds.Count >= 1) {
                switch (cmds[0].toLowerCase()) {
                    case "cmd": case "运行": case "命令": case "运行命令":
                        if (JudgePermission(e)) { 
                            if (cmds.Count < 2) {
                                e.feedback("参数不足！")
                            } else if (cmds.Count < 3) {//只有两个参数,如：/cmd listd
                                MCConnections.forEach(eachCon => {
                                    const ServerName = eachCon.Tag.name;
                                    eachCon.RunCmd(cmds[1], function (result) {
                                        e.feedback(ServerName + "执行结果:" + result.trim())
                                    });
                                });
                            } else if (cmds.Count < 4) {//有三个参数,如：/cmd 生存服 "testfor @a"
                                let executed = false;
                                MCConnections.forEach(eachCon => {
                                    if (executed) return;
                                    const ServerName = eachCon.Tag.name;
                                    if (ServerName == cmds[1]) {
                                        eachCon.RunCmd(cmds[2], function (result) {
                                            e.feedback(ServerName + "执行结果:" + result.trim())
                                        });
                                        executed = true;
                                    }
                                });
                                if (!executed) {
                                    e.feedback(`未匹配到服务器:${cmds[1]}`)
                                }
                            } else {
                                e.feedback(`没有${cmds.Count}个参数的重载！\n如：/cmd listd\n或：/cmd 生存服 "testfor @a"`)
                            }
                        } else {
                            e.feedback("无权限！")
                        }

                }

            }
        }
    }
})
