moduleInfo.Author = "littlegao233"
moduleInfo.Version = "v0.0.1"
moduleInfo.Description = '群内管理员使用"/白名单 添加 [服务器] <玩家名>"添加白名单\
群内使用"/白名单 [服务器] <玩家名>'
//管理员QQ请在main.js配置
const events = importNamespace('PFBridgeCore').APIs.Events
const api = importNamespace('PFBridgeCore').APIs.API
const MCConnections = importNamespace('PFBridgeCore').ConnectionList.MCConnections
const Thread = importNamespace('System.Threading').Thread
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
                switch (cmds[0].toLowerCase()) {
                    case "白名单": case "whitelist":
                        if (ConfigAdminQQs.some(l => l == senderId)) { //判断权限
                            if (cmds.Count < 2) {
                                e.feedback("参数不足！")
                            } else {
                                switch (cmds[1].toLowerCase()) {
                                    case "添加": case "add":
                                        if (cmds.Count < 3) {
                                            e.feedback("参数不足！")
                                        } else {
                                            if (cmds.Count < 4) {//只有三个参数,如：/白名单 添加 "kun kun"
                                                let AllResult = new Array()

                                                MCConnections.forEach(eachCon => {
                                                    const ServerName = eachCon.Tag.name;
                                                    AllResult.push([ServerName, "添加结果未知", false])
                                                    eachCon.RunCmd(`whitelist add ${cmds[2]}`, function (result) {
                                                        let item = AllResult.find(x => x[0] === ServerName)
                                                        item[1] = result.trim();
                                                        item[2] = true;
                                                    });
                                                });
                                                let waitTimes = 0
                                                async function waitForResult() {
                                                    if (waitTimes < 50 && AllResult.some(x => x[2] === false)) {
                                                        Thread.Sleep(100)
                                                        waitTimes++;
                                                        waitForResult();
                                                    } else {
                                                        if (AllResult.length === 0) {
                                                            e.feedback("未添加到任何服务器");
                                                        } else {
                                                            let outputResult = [`["${cmds[2]}"添加结果]`];
                                                            AllResult.forEach(x => outputResult.push(`${x[0]}:${x[1]}`));
                                                            e.feedback(outputResult.join("\n"));
                                                        }
                                                    }
                                                }
                                                waitForResult()
                                            } else if (cmds.Count < 5) {//有三个参数,如：/白名单 添加 生存服 "kun kun"
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
                                                e.feedback(`没有${cmds.Count}个参数的重载！\n如：/白名单 添加 "kun kun"\n或：/白名单 添加 生存服 "kun kun"`)
                                            }
                                        }
                                    case "删除": case "del": case "remove":
                                    default:
                                }

                            }
                        } else {
                            e.feedback("无权限！")
                        }
                }
            }
        }
    }
})
