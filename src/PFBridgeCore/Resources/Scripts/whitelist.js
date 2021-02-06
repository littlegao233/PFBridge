moduleInfo.Author = "littlegao233"
moduleInfo.Version = "v0.0.1"
moduleInfo.Description = '群内管理员使用"/白名单 添加 [服务器] <玩家名>"添加白名单\n群内使用"/白名单 [服务器] <玩家名>'
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
                                e.feedback("参数不足！\n/白名单 <添加|删除> [服务器] <玩家名>")
                            } else {
                                switch (cmds[1].toLowerCase()) {
                                    case "添加": case "add":
                                        if (cmds.Count < 3) {
                                            e.feedback('参数不足！\n如/白名单 添加 "xiao A"\n或/白名单 添加 生存服 "xiao B"')
                                        } else {
                                            if (cmds.Count < 4) {//只有三个参数,如：/白名单 添加 "kun kun"
                                                let AllResult = new Array()
                                                let ServersNeedToAdd = new Array()
                                                MCConnections.forEach(x => {
                                                    try {
                                                        if (x.Tag.WhitelistEnabled === false) return;//如果白名单关闭就跳过
                                                    } catch (error) { }
                                                    ServersNeedToAdd.push(x)//增加到要处理的服务器
                                                });
                                                let TotalCount = ServersNeedToAdd.length
                                                let SuccessCount = 0
                                                let FailedCount = 0
                                                for (let i = 0; i < TotalCount; i++) {
                                                    let thisCon = ServersNeedToAdd[i]
                                                    if (thisCon.State) {
                                                        const ServerName = thisCon.Tag.name;
                                                        AllResult.push([ServerName, "添加结果未知"])
                                                        thisCon.RunCmd(`whitelist add "${cmds[2]}"`, function (result) {
                                                            let item = AllResult.find(x => x[0] == ServerName)
                                                            item[1] = result.trim();
                                                            if (item[1] === "Player added to whitelist") {
                                                                item[1] = "白名单添加成功"
                                                            } else if (item[1] === "Player already in whitelist") {
                                                                item[1] = "玩家已存在于白名单"
                                                            }
                                                            SuccessCount++;
                                                            if (TotalCount == SuccessCount + FailedCount && SuccessCount != 0) {//判断是否是执行的最后一个服务器
                                                                let outputResult = [`["${cmds[2]}"添加结果]`];
                                                                AllResult.forEach(x => outputResult.push(`${x[0]}:${x[1]}`));
                                                                e.feedback(outputResult.join("\n"));
                                                            }
                                                        });
                                                    } else {
                                                        FailedCount++;
                                                    }
                                                }
                                                if (FailedCount == TotalCount || TotalCount === 0) {
                                                    e.feedback("未添加到任何服务器");
                                                }
                                            } else if (cmds.Count < 5) {//有三个参数,如：/白名单 添加 生存服 "kun kun"
                                                let CannotMatch = true;
                                                for (let i = 0; i < MCConnections.Count; i++) {
                                                    let thisCon = MCConnections[i]
                                                    const ServerName = thisCon.Tag.name;
                                                    if (ServerName == cmds[2]) {
                                                        CannotMatch = false;
                                                    } else { continue; }
                                                    if (thisCon.State) {
                                                        thisCon.RunCmd(`whitelist add "${cmds[3]}"`, function (result) {
                                                            let returnStr = result.trim();
                                                            if (returnStr === "Player added to whitelist") {
                                                                returnStr = "添加到白名单成功";
                                                            } else if (returnStr === "Player already in whitelist") {
                                                                returnStr = "已存在于白名单";
                                                            }
                                                            e.feedback(`[${ServerName}]${cmds[3]}${returnStr}`);
                                                        });
                                                    } else {
                                                        e.feedback(`[${ServerName}]服务器离线，无法添加`);
                                                    }
                                                }
                                                if (CannotMatch) {
                                                    e.feedback(`未匹配到服务器:${cmds[3]}`)
                                                }
                                            } else {
                                                e.feedback(`没有${cmds.Count}个参数的重载！\n如：/白名单 添加 "kun kun"\n或：/白名单 添加 生存服 "kun kun"`)
                                            }
                                        }
                                        break;
                                    case "删除": case "del": case "remove":
                                        if (cmds.Count < 3) {
                                            e.feedback('参数不足！\n如/白名单 删除 "xiao A"\n或/白名单 删除 生存服 "xiao B"')
                                        } else {
                                            if (cmds.Count < 4) {//只有三个参数,如：/白名单 删除 "kun kun"
                                                let AllResult = new Array()
                                                let ServersNeedToAdd = new Array()
                                                MCConnections.forEach(x => {
                                                    try {
                                                        if (x.Tag.WhitelistEnabled === false) return;//如果白名单关闭就跳过
                                                    } catch (error) { }
                                                    ServersNeedToAdd.push(x)//增加到要处理的服务器
                                                });
                                                let TotalCount = ServersNeedToAdd.length
                                                let SuccessCount = 0
                                                let FailedCount = 0
                                                for (let i = 0; i < TotalCount; i++) {
                                                    let thisCon = ServersNeedToAdd[i]
                                                    if (thisCon.State) {
                                                        const ServerName = thisCon.Tag.name;
                                                        AllResult.push([ServerName, "删除结果未知"])
                                                        thisCon.RunCmd(`whitelist remove "${cmds[2]}"`, function (result) {
                                                            let item = AllResult.find(x => x[0] == ServerName)
                                                            item[1] = result.trim();
                                                            if (item[1] === "Player removed from whitelist") {
                                                                item[1] = "白名单删除成功"
                                                            } else if (item[1] === "Player not in whitelist") {
                                                                item[1] = "玩家不在白名单中"
                                                            }
                                                            SuccessCount++;
                                                            if (TotalCount == SuccessCount + FailedCount && SuccessCount != 0) {//判断是否是执行的最后一个服务器
                                                                let outputResult = [`["${cmds[2]}"删除结果]`];
                                                                AllResult.forEach(x => outputResult.push(`${x[0]}:${x[1]}`));
                                                                e.feedback(outputResult.join("\n"));
                                                            }
                                                        });
                                                    } else {
                                                        FailedCount++;
                                                    }
                                                }
                                                if (FailedCount == TotalCount || TotalCount === 0) {
                                                    e.feedback("未删除到任何服务器");
                                                }
                                            } else if (cmds.Count < 5) {//有三个参数,如：/白名单 删除 生存服 "kun kun"
                                                let CannotMatch = true;
                                                for (let i = 0; i < MCConnections.Count; i++) {
                                                    let thisCon = MCConnections[i]
                                                    const ServerName = thisCon.Tag.name;
                                                    if (ServerName == cmds[2]) {
                                                        CannotMatch = false;
                                                    } else { continue; }
                                                    if (thisCon.State) {
                                                        thisCon.RunCmd(`whitelist remove "${cmds[3]}"`, function (result) {
                                                            let returnStr = result.trim();
                                                            if (returnStr === "Player removed from whitelist") {
                                                                returnStr = "白名单删除成功"
                                                            } else if (returnStr === "Player not in whitelist") {
                                                                returnStr = "玩家不在白名单中"
                                                            }
                                                            e.feedback(`[${ServerName}]${cmds[3]}${returnStr}`);
                                                        });
                                                    } else {
                                                        e.feedback(`[${ServerName}]服务器离线，无法删除`);
                                                    }
                                                }
                                                if (CannotMatch) {
                                                    e.feedback(`未匹配到服务器:${cmds[3]}`)
                                                }
                                            } else {
                                                e.feedback(`没有${cmds.Count}个参数的重载！\n如：/白名单 删除 "kun kun"\n或：/白名单 删除 生存服 "kun kun"`)
                                            }
                                        }
                                        break;
                                    default:
                                }
                            }
                        } else {
                            e.feedback("无权限！")
                        }
                        break;
                }
            }
        }
    }
})
