moduleInfo.Author = "mcllaop"
moduleInfo.Version = "v0.0.1"
moduleInfo.Description = "[BDX专用]\n群内使用/tps命令返回TPS"


const MCConnections = importNamespace('PFBridgeCore').ConnectionList.MCConnections
const api = importNamespace('PFBridgeCore').APIs.API
const events = importNamespace('PFBridgeCore').APIs.Events

const Engine = importNamespace('PFBridgeCore').Main.Engine
const Data_GetConfigGroups = Engine.GetShareData("GetConfigGroups")
function GetConfigGroups() { return Data_GetConfigGroups.Value();}
//const Data_GetConfigAdminQQs = Engine.GetShareData("GetConfigAdminQQs")
//function GetConfigAdminQQs() { return Data_GetConfigAdminQQs.Value();}
events.QQ.onGroupMessage.add(function (e) {
    const { groupId } = e
    let index = GetConfigGroups().findIndex(l => l.id == groupId);//匹配群号（于配置）
    if (index !== -1) {
        //let group = ConfigGroups[index];
        let msg = e.message;
        const { senderId, message } = e
        if (message.startsWith('/') || message.startsWith('+')) {//判断消息前缀
            let cmds = e.messageMatch.getCommands("/", "+")//使用现成的匹配方法
            if (cmds.Count >= 1) {
                switch (cmds[0]) {
                    case "tps":
                        MCConnections.forEach(eachCon => {
                            const ServerName = eachCon.Tag.name;
                            if (ServerName == "TFC") {
                                eachCon.RunCmd("tps", function (result) {
                                    e.feedback(ServerName + "查询结果:\n" + result.trim())
                                });
                            }
                        });
                }
            }
        }
    }
})
