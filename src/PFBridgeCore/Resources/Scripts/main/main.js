moduleInfo.Author = "littlegao233";
moduleInfo.Version = "v0.0.2";
moduleInfo.Description = "包含配置文件的读写、\n服务器之间的同步、\n群与服务器的聊天同步、\n加入服务器的群反馈";
let AdminQQs = new Array();
let Groups = new Array();
let Servers = new Array();
//#region 共享数据
var engine = importNamespace('PFBridgeCore').Main.Engine;
engine.SetShareData("GetConfigGroups", () => { return Groups; });
engine.SetShareData("GetConfigAdminQQs", () => { return AdminQQs; });
//#endregion
//#region >>>>>-----公共方法(建议折叠)----->>>>>
var ConnectionManager = importNamespace('PFBridgeCore').ConnectionManager;
var events = importNamespace('PFBridgeCore').APIs.Events;
var api = importNamespace('PFBridgeCore').APIs.API;
var MCConnections = importNamespace('PFBridgeCore').ConnectionList.MCConnections;
//保存文件
var File = System.IO.File; //导入命名空间
var Path = System.IO.Path;
var configPath = Path.Combine(api.PluginDataPath, "config.json");
function LoadConfig() {
    const JSONLinq = importNamespace('Newtonsoft.Json.Linq'); //导入命名空间
    //用于读取带注释的json
    const imported = JSON.parse(JSONLinq.JObject.Parse(File.ReadAllText(configPath)).ToString());
    AdminQQs = imported.AdminQQs;
    Groups = imported.Groups;
    Servers = imported.Servers;
}
if (File.Exists(configPath)) {
    LoadConfig();
    api.Log("从" + configPath + "读取配置文件成功");
}
else { //输出默认配置文件
    const willexport = "{\n    \"AdminQQs\": [441870948, 233333]/*管理员QQ号,用于配置是否可执行命令等*/,\n    \"Groups\": [\n        {\n            \"id\": 626872357,//QQ群号\n            \"ServerMsgToGroup\": true,//是否转发服务器的各种消息到该群\n            \"GroupMsgToServer\": true//是否将该群的消息转发到所有服务器\n        }\n    ],\n    \"Servers\": [\n        {\n            \"type\": \"websocket\",\n            \"url\": \"ws://127.0.0.1:29132/mcws\",//websocket地址|如{\"Port\": \"29132\",\"EndPoint\": \"mcws\",\"Password\": \"commandpassword\"}对应ws://127.0.0.1:29132/mcws\n            \"token\": \"commandpassword\",//websocket密匙串（用于运行命令等操作）|\"Password\": \"commandpassword\"\n            \"name\": \"测试服务器\",\n            \"ServerMsgToGroup\": true,//是否将该服务器的各种消息转发到群\n            \"GroupMsgToServer\": true,//是否转发群消息到该服务器\n            \"ServerMsgToOther\": true,//是否将该服务器的各种消息转发到其他已连接服服务器（多服联动）\n            \"ReceiveMsgFromOther\": true,//是否接受其他服务器的消息（多服联动）\n            \"WhitelistEnabled\": true//是否开启白名单，改参数主要在whitelist.js中用到\n        }/*, {//在这里添加多个服务器\n            \"type\": \"websocket\",\n            \"url\": \"ws://127.0.0.1:29132/mcws\",//websocket地址\n            \"token\": \"commandpassword\",//websocket密匙串（用于运行命令等操作）\n        }*/\n    ]\n}";
    File.WriteAllText(configPath, willexport);
    api.Log("已输出默认配置文件到" + configPath);
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
    return ConnectionManager.AddWebsocketClient(url, token, tag);
}
/**
 * 发送消息到所有已经连接并且配置开启GroupMsgToServer的MC服务器
 * @param {string} message 消息内容
 */
function SendBoardcastToAllServer(message) {
    MCConnections.forEach(connection => {
        let index = Servers.findIndex(s => s.id === connection.Id);
        if (index !== -1) {
            let server = Servers[index];
            if (server.GroupMsgToServer) {
                SendToServer(connection, message);
            }
        }
    });
}
/**
 * 发送消息到指定服务器
 * @param {*} connection 连接实例
 * @param {string} message 消息内容
 */
function SendToServer(connection, message) {
    connection.RunCmd(`tellraw @a {"rawtext":[{"text":"${encodeUnicode(message)}"}]}`);
}
/**
 * string转为unicode编码
 * @param {string} str 内容
 */
var encodeUnicode = function (str) {
    //return escape(str).replace(/%u/g, "\\u")
    var res = [];
    for (var i = 0; i < str.length; i++) {
        res[i] = ("00" + str.charCodeAt(i).toString(16)).slice(-4);
    }
    return "\\u" + res.join("\\u");
};
//#endregion <<<<<-----公共方法(建议折叠)-----<<<<<
//#region 服务器主体部分
Servers.forEach(server => {
    if (server.type = "websocket") {
        server.id = AddWebsocket(server.url, server.token, server);
    }
    else {
        api.LogErr("未知的mc连接方案:" + server.type);
    }
});
events.MC.Chat.Add(function (e) {
    const { connection, sender, message } = e;
    const { Id } = connection;
    let index = Servers.findIndex(s => s.id === Id); //匹配服务器（于配置中）
    if (index !== -1) {
        let server = Servers[index];
        if (server.ServerMsgToGroup) {
            ProcessServerMsgToGroup(`[${server.name}:Chat]${sender}>${message}`);
        }
        if (server.ServerMsgToOther) {
            ProcessServerMsgToOtherServer(Id, `§b【${server.name}消息】§e<${sender}>§a${message}`);
        }
    }
});
//events.MC.Cmd.add(function (e) {
//        const { connection, sender, cmd } = e
//        const { Id } = connection
//        ProcessServerMsgToGroup(JSON.stringify(e));
//})
events.MC.Join.Add(function (e) {
    const { connection, sender, ip, uuid, xuid } = e;
    const { Id } = connection;
    let index = Servers.findIndex(s => s.id === Id); //匹配服务器（于配置中）
    if (index !== -1) {
        let server = Servers[index];
        if (server.ServerMsgToGroup) {
            ProcessServerMsgToGroup(`[${server.name}:Join]${sender}加入了服务器`);
        }
        if (server.ServerMsgToOther) {
            ProcessServerMsgToOtherServer(Id, `§b【${server.name}:Join】§e${sender}§a加入了服务器`);
        }
    }
});
events.MC.Left.Add(function (e) {
    const { connection, sender, ip, uuid, xuid } = e;
    const { Id } = connection;
    let index = Servers.findIndex(s => s.id === Id); //匹配服务器（于配置中）
    if (index !== -1) {
        let server = Servers[index];
        if (server.ServerMsgToGroup) {
            ProcessServerMsgToGroup(`[${server.name}:Left]${sender}离开了服务器`);
        }
        if (server.ServerMsgToOther) {
            ProcessServerMsgToOtherServer(Id, `§b【${server.name}:Left】§e${sender}§a离开了服务器`);
        }
    }
});
const entityList = {
    "entity.player.name": "玩家",
    "entity.area_effect_cloud.name": "区域效果云雾",
    "entity.armor_stand.name": "盔甲架",
    "entity.arrow.name": "箭",
    "entity.bat.name": "蝙蝠",
    "entity.bee.name": "蜜蜂",
    "entity.blaze.name": "烈焰人",
    "entity.boat.name": "船",
    "entity.cat.name": "猫",
    "entity.cave_spider.name": "洞穴蜘蛛",
    "entity.chicken.name": "鸡",
    "entity.cow.name": "牛",
    "entity.creeper.name": "爬行者",
    "entity.dolphin.name": "海豚",
    "entity.panda.name": "熊猫",
    "entity.donkey.name": "驴",
    "entity.dragon_fireball.name": "末影龙火球",
    "entity.drowned.name": "溺尸",
    "entity.egg.name": "鸡蛋",
    "entity.elder_guardian.name": "远古守卫者",
    "entity.ender_crystal.name": "末影水晶",
    "entity.ender_dragon.name": "末影龙",
    "entity.enderman.name": "末影人",
    "entity.endermite.name": "末影螨",
    "entity.ender_pearl.name": "末影珍珠",
    "entity.evocation_illager.name": "唤魔者",
    "entity.evocation_fang.name": "唤魔者尖牙",
    "entity.eye_of_ender_signal.name": "末影之眼",
    "entity.falling_block.name": "下落的方块",
    "entity.fireball.name": "火球",
    "entity.fireworks_rocket.name": "焰火火箭",
    "entity.fishing_hook.name": "鱼钩",
    "entity.fish.clownfish.name": "海葵鱼",
    "entity.fox.name": "狐狸",
    "entity.cod.name": "鳕鱼",
    "entity.pufferfish.name": "河豚",
    "entity.salmon.name": "鲑鱼",
    "entity.tropicalfish.name": "热带鱼",
    "entity.ghast.name": "恶魂",
    "entity.piglin_brute.name": "残暴 Piglin",
    "entity.guardian.name": "守卫者",
    "entity.hoglin.name": "Hoglin",
    "entity.horse.name": "马",
    "entity.husk.name": "尸壳",
    "entity.ravager.name": "劫掠兽",
    "entity.iron_golem.name": "铁傀儡",
    "entity.item.name": "物品",
    "entity.leash_knot.name": "拴绳结",
    "entity.lightning_bolt.name": "闪电",
    "entity.lingering_potion.name": "滞留药水",
    "entity.llama.name": "羊驼",
    "entity.llama_spit.name": "羊驼口水",
    "entity.magma_cube.name": "岩浆怪",
    "entity.minecart.name": "矿车",
    "entity.chest_minecart.name": "运输矿车",
    "entity.command_block_minecart.name": "命令方块矿车",
    "entity.furnace_minecart.name": "动力矿车",
    "entity.hopper_minecart.name": "漏斗矿车",
    "entity.tnt_minecart.name": "TNT 矿车",
    "entity.mule.name": "骡子",
    "entity.mooshroom.name": "哞菇",
    "entity.moving_block.name": "移动中的方块",
    "entity.ocelot.name": "豹猫",
    "entity.painting.name": "画",
    "entity.parrot.name": "鹦鹉",
    "entity.phantom.name": "幻翼",
    "entity.pig.name": "猪",
    "entity.piglin.name": "Piglin",
    "entity.pillager.name": "掠夺者",
    "entity.polar_bear.name": "北极熊",
    "entity.rabbit.name": "兔子",
    "entity.sheep.name": "羊",
    "entity.shulker.name": "潜影贝",
    "entity.shulker_bullet.name": "潜影贝子弹",
    "entity.silverfish.name": "蠹虫",
    "entity.skeleton.name": "骷髅",
    "entity.skeleton_horse.name": "骷髅马",
    "entity.stray.name": "流浪者",
    "entity.slime.name": "史莱姆",
    "entity.small_fireball.name": "小火球",
    "entity.snowball.name": "雪球",
    "entity.snow_golem.name": "雪傀儡",
    "entity.spider.name": "蜘蛛",
    "entity.splash_potion.name": "药水",
    "entity.squid.name": "鱿鱼",
    "entity.strider.name": "炽足兽",
    "entity.tnt.name": "TNT 方块",
    "entity.thrown_trident.name": "三叉戟",
    "entity.tripod_camera.name": "三脚架摄像机",
    "entity.turtle.name": "海龟",
    "entity.unknown.name": "未知",
    "entity.vex.name": "恼鬼",
    "entity.villager.name": "村民",
    "entity.villager.armor": "盔甲匠",
    "entity.villager.butcher": "屠夫",
    "entity.villager.cartographer": "制图师",
    "entity.villager.cleric": "牧师",
    "entity.villager.farmer": "农民",
    "entity.villager.fisherman": "渔夫",
    "entity.villager.fletcher": "制箭师",
    "entity.villager.leather": "皮匠",
    "entity.villager.librarian": "图书管理员",
    "entity.villager.shepherd": "牧羊人",
    "entity.villager.tool": "工具匠",
    "entity.villager.weapon": "武器匠",
    "entity.villager.mason": "石匠",
    "entity.villager.unskilled": "不熟练的村民",
    "entity.villager_v2.name": "村民",
    "entity.vindicator.name": "卫道士",
    "entity.wandering_trader.name": "流浪商人",
    "entity.witch.name": "女巫",
    "entity.wither.name": "凋灵",
    "entity.wither_skeleton.name": "凋灵骷髅",
    "entity.wither_skull.name": "凋灵头颅",
    "entity.wither_skull_dangerous.name": "凋灵头颅",
    "entity.wolf.name": "狼",
    "entity.xp_orb.name": "经验球",
    "entity.xp_bottle.name": "附魔之瓶",
    "entity.zoglin.name": "僵尸疣猪兽",
    "entity.zombie.name": "僵尸",
    "entity.zombie_horse.name": "僵尸马",
    "entity.zombie_pigman.name": "僵尸 Piglin",
    "entity.zombie_villager.name": "僵尸村民",
    "entity.zombie_villager_v2.name": "怪人村民"
};
function GetEntityName(id) {
    try {
        return entityList[id];
    }
    catch (e) {
        try {
            return id.substr(7, id.length - 12);
        }
        catch (e) {
            return "未知生物";
        }
    }
}
events.MC.MobDie.Add(function (e) {
    const { connection, mobname, mobtype, dmcase, srcname, srctype, pos } = e;
    const { Id } = connection;
    let index = Servers.findIndex(s => s.id === Id); //匹配服务器（于配置中）
    if (index !== -1) {
        let server = Servers[index];
        if (server.ServerMsgToGroup || server.ServerMsgToOther) {
            let msg = null;
            if (mobtype === "entity.player.name") {
                if (srctype === "entity.player.name") {
                    msg = `玩家${mobname}被玩家${srcname}杀死了`;
                }
                else if (srctype === "") {
                    msg = `玩家${mobname}不知怎么地就一命呜呼了！`;
                }
                else if (srcname === "") {
                    msg = `玩家${mobname}被${GetEntityName(srctype)}杀死了`;
                }
                else {
                    msg = `玩家${mobname}被${srcname}(${GetEntityName(srctype)})杀死了`;
                }
            }
            else {
                if (mobtype !== "" && mobname !== "") {
                    if (srctype === "") {
                        msg = `${GetEntityName(mobtype)}(${mobname})不知怎么地就一命呜呼了`;
                    }
                    else if (srcname === "") {
                        msg = `${mobname}(${GetEntityName(mobtype)})被${GetEntityName(srctype)}杀死了`;
                    }
                    else {
                        msg = `${mobname}(${GetEntityName(mobtype)})被${srcname}(${GetEntityName(srctype)})杀死了`;
                    }
                }
            }
            if (msg !== null) {
                if (server.ServerMsgToGroup) {
                    ProcessServerMsgToGroup(`[${server.name}:Die]${msg}`);
                }
                if (server.ServerMsgToOther) {
                    ProcessServerMsgToOtherServer(Id, `§b【${server.name}:Left】§d${msg}`);
                }
            }
        }
    }
});
function ProcessServerMsgToGroup(message) {
    Groups.forEach(group => {
        if (group.ServerMsgToGroup) {
            api.SendGroupMessage(group.id, message);
        }
    });
}
function ProcessServerMsgToOtherServer(id, message) {
    MCConnections.forEach(connection => {
        if (connection.Id !== id) {
            let index = Servers.findIndex(s => s.id === connection.Id);
            if (index !== -1) {
                let server = Servers[index];
                if (server.ReceiveMsgFromOther) {
                    SendToServer(connection, `${message}`);
                }
            }
        }
    });
}
//#endregion
//#region QQ主体部分
events.IM.OnGroupMessage.Add(function (e) {
    const { groupId } = e;
    let index = Groups.findIndex(l => l.id == groupId); //匹配群号（于配置）
    if (index !== -1) {
        let group = Groups[index];
        const { /* senderId,*/ message } = e;
        if (message.startsWith('/') || message.startsWith('+')) {
        }
        else {
            if (group.GroupMsgToServer) {
                const { groupName, /*senderNick, */ memberCard, parsedMessage } = e;
                let msg = `§b【${groupName}】§e<${((memberCard === null || memberCard === "") ? e.senderNick : memberCard)}>§a${parsedMessage}`;
                SendBoardcastToAllServer(msg);
            }
            //api.SendPrivateMessageFromGroup(e.fromGroup, e.fromQQ, "test:" + e.message)
            //api.SendGroupMessage(e.fromGroup, "test1:" + e.message)
        }
    }
});
//#endregion
