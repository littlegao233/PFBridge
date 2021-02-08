moduleInfo.Author = "littlegao233"
moduleInfo.Version = "v0.0.2"
moduleInfo.Description = '群内管理员使用"/白名单 <添加|删除> [服务器] <玩家名>"操作白名单\n群内使用"/白名单 查询 [服务器] <玩家名>查询白名单'
//简单设置：
const JudgePermissionByConfig = true;
//JudgePermissionByConfig：是否通过配置文件判断权限
//  - true：仅有main.js配置的管理员QQ课执行
//  - false：发送者为群内的管理员就能执行
function JudgePermission(e) {
    if (JudgePermissionByConfig) {
        const { senderId } = e;
        //根据配置文件main.js中的管理员判断权限
        return ConfigAdminQQs.some(l => l == senderId);
    } else {
        //根据成员类型判断权限（memberType属性=>0未知;1成员;2管理员;3群主）
        return e.memberType >= 2
    }
}
const events = importNamespace('PFBridgeCore').APIs.Events
const api = importNamespace('PFBridgeCore').APIs.API
const MCConnections = importNamespace('PFBridgeCore').ConnectionList.MCConnections
const Thread = importNamespace('System.Threading').Thread
events.QQ.onGroupMessage.add(function (e) {
    const { groupId } = e
    let index = GetConfigGroups().findIndex(l => l.id == groupId);//匹配群号（于配置）
    if (index !== -1) {
        //let group = GetConfigGroups()[index];
        //let msg = e.message;
        const { message } = e
        if (message.startsWith('/') || message.startsWith('+')) {//判断消息前缀
            let cmds = e.messageMatch.getCommands("/", "+")//使用现成的匹配方法
            if (cmds.Count >= 1) {
                switch (cmds[0].toLowerCase()) {
                    case "白名单": case "whitelist":
                        if (cmds.Count < 2) {
                            e.feedback("参数不足！\n/白名单 <添加|删除|查询> [服务器] <玩家名>")
                        } else {
                            switch (cmds[1].toLowerCase()) {
                                case "添加": case "add":
                                    if (JudgePermission(e)) {
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
                                                    const thisCon = ServersNeedToAdd[i]
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
                                                    const thisCon = MCConnections[i]
                                                    const ServerName = thisCon.Tag.name;
                                                    if (ServerName == cmds[2]) {
                                                        CannotMatch = false;
                                                    } else { continue; }
                                                    if (thisCon.State) {
                                                        let WhitelistEnabled = true
                                                        try { WhitelistEnabled = thisCon.Tag.WhitelistEnabled } catch (error) { }
                                                        if (WhitelistEnabled) {
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
                                                            e.feedback(`[${ServerName}]未开启白名单，无法添加`);
                                                        }
                                                    } else {
                                                        e.feedback(`[${ServerName}]服务器离线，无法添加`);
                                                    }
                                                }
                                                if (CannotMatch) {
                                                    e.feedback(`未匹配到服务器:${cmds[2]}`)
                                                }
                                            } else {
                                                e.feedback(`没有${cmds.Count}个参数的重载！\n如：/白名单 添加 "kun kun"\n或：/白名单 添加 生存服 "kun kun"`)
                                            }
                                        }
                                    } else {
                                        e.feedback("无权限！")
                                    }
                                    break;
                                case "删除": case "del": case "remove":
                                    if (JudgePermission(e)) {
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
                                                    const thisCon = ServersNeedToAdd[i]
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
                                                    const thisCon = MCConnections[i]
                                                    const ServerName = thisCon.Tag.name;
                                                    if (ServerName == cmds[2]) {
                                                        CannotMatch = false;
                                                    } else { continue; }
                                                    if (thisCon.State) {
                                                        let WhitelistEnabled = true
                                                        try { WhitelistEnabled = thisCon.Tag.WhitelistEnabled } catch (error) { }
                                                        if (WhitelistEnabled) {
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
                                                            e.feedback(`[${ServerName}]未开启白名单，无法删除`);
                                                        }
                                                    } else {
                                                        e.feedback(`[${ServerName}]服务器离线，无法删除`);
                                                    }
                                                }
                                                if (CannotMatch) {
                                                    e.feedback(`未匹配到服务器:${cmds[2]}`)
                                                }
                                            } else {
                                                e.feedback(`没有${cmds.Count}个参数的重载！\n如：/白名单 删除 "kun kun"\n或：/白名单 删除 生存服 "kun kun"`)
                                            }
                                        }
                                    } else {
                                        e.feedback("无权限！")
                                    }
                                    break;
                                case "查询": case "query":
                                    //默认所有成员都能查询白名单
                                    if (cmds.Count < 3) {
                                        e.feedback('参数不足！\n如/白名单 查询 "xiao A"\n或/白名单 查询 生存服 "xiao B"')
                                    } else {
                                        if (cmds.Count < 4) {//只有三个参数,如：/白名单 查询 "kun kun"
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
                                                    AllResult.push([ServerName, "查询结果未知"])
                                                    thisCon.RunCmd('whitelist list', function (result) {
                                                        try {
                                                            const matchResult = /###\*((\s|\S)*)\*###/.exec(result)
                                                            //###* {"command":"whitelist","result":[{"name":"gxh2004"}]}*###
                                                            const wl = JSON.parse(matchResult[1]).result
                                                            let item = AllResult.find(x => x[0] == ServerName)
                                                            if (wl.some(x => x.name == cmds[2])) {//查到
                                                                item[1] = "已在白名单中"
                                                            } else {//未查到
                                                                item[1] = "不在白名单中"
                                                            }
                                                            SuccessCount++;
                                                            if (TotalCount == SuccessCount + FailedCount && SuccessCount != 0) {//判断是否是执行的最后一个服务器
                                                                let outputResult = [`["${cmds[2]}"查询结果]`];
                                                                AllResult.forEach(x => outputResult.push(`${x[0]}:${x[1]}`));
                                                                e.feedback(outputResult.join("\n"));
                                                            }
                                                        } catch (error) { }
                                                    });
                                                } else {
                                                    FailedCount++;
                                                }
                                            }
                                            if (FailedCount == TotalCount || TotalCount === 0) {
                                                e.feedback("未查询到任何服务器");
                                            }
                                        } else if (cmds.Count < 5) {//有三个参数,如：/白名单 查询 生存服 "kun kun"
                                            let CannotMatch = true;
                                            for (let i = 0; i < MCConnections.Count; i++) {
                                                let thisCon = MCConnections[i]
                                                const ServerName = thisCon.Tag.name;
                                                if (ServerName == cmds[2]) {
                                                    CannotMatch = false;
                                                } else { continue; }
                                                if (thisCon.State) {
                                                    let WhitelistEnabled = true
                                                    try { WhitelistEnabled = thisCon.Tag.WhitelistEnabled } catch (error) { }
                                                    if (WhitelistEnabled) {
                                                        thisCon.RunCmd('whitelist list', function (result) {
                                                            const matchResult = /###\*((\s|\S)*)\*###/.exec(result)
                                                            //###* {"command":"whitelist","result":[{"name":"gxh2004"}]}*###
                                                            const wl = JSON.parse(matchResult[1]).result
                                                            if (wl.some(x => x.name == cmds[3])) {//查到
                                                                e.feedback(`[${ServerName}]${cmds[3]}已在白名单中`);
                                                            } else {//未查到
                                                                e.feedback(`[${ServerName}]${cmds[3]}不在白名单中`);
                                                            }
                                                        });
                                                    }
                                                    else {
                                                        e.feedback(`[${ServerName}]未开启白名单，无法查询`);
                                                    }
                                                } else {
                                                    e.feedback(`[${ServerName}]服务器离线，无法查询`);
                                                }
                                            }
                                            if (CannotMatch) {
                                                e.feedback(`未匹配到服务器:${cmds[2]}`)
                                            }
                                        } else {
                                            e.feedback(`没有${cmds.Count}个参数的重载！\n如：/白名单 查询 "kun kun"\n或：/白名单 查询 生存服 "kun kun"`)
                                        }
                                    }
                                    break;
                                default:
                            }
                        }
                        break;
                }
            }
        }
    }
})
