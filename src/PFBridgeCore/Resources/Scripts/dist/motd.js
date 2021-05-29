"use strict";
/// <reference types="PFBridgeCore" />
moduleInfo.Author = "littlegao233";
moduleInfo.Description = "Motd插件，使用方法：群内/motd ip:port查询服务器";
moduleInfo.Version = "0.0.1";
const sendData = System.Convert.FromBase64String("AQAAAAAAA2oHAP//AP7+/v79/f39EjRWeJx0FrwC/0lw");
const apis = importNamespace("PFBridgeCore" /* Core */).APIs;
const api = apis.API;
const utils = importNamespace("PFBridgeCore.Utils.Net.Sockets" /* UtilsNetSockets */);
const SocketApi = utils.Socket;
const TaskRun = importNamespace('System.Threading.Tasks').Task.Run;
/*
0:MCPE
1:Dedicated Server
2:431
3:1.16.220
4:0
5:10
6:9761407514957423030
7:Bedrock level
8:Survival
9:1
10:19130
11:19131
12:
*/
class MotdInfo {
    constructor(_success, strs) {
        this.success = _success;
        if (_success) {
            this.detail = new MotdDetail(strs);
        }
        else {
            this.msg = strs.join("\n");
        }
    }
}
class MotdDetail {
    constructor(strs) {
        this.platform = strs[0];
        this.description = strs[1];
        this.protocolVersion = strs[2];
        this.gameVersion = strs[3];
        this.currentPlayer = strs[4];
        this.maxPlayer = strs[5];
        this.unknown1 = strs[6];
        this.levelName = strs[7];
        this.gameMode = strs[8];
        this.unknown2 = strs[9];
        this.ipv4Port = strs[10];
        this.ipv6Port = strs[11];
    }
}
/**
 * Motd协议查询服务器信息
 * @param ip IP地址
 * @param port 端口
 */
function MotdBE(ip, port) {
    try {
        let client = SocketApi.CreateSocket(2 /* Dgram */, 17 /* Udp */);
        SocketApi.SendData(client, sendData, ip, port);
        const receive = SocketApi.ReceiveData(client, 0, 256, 0 /* None */).slice(35);
        const strs = System.Text.Encoding.UTF8.GetString(receive).split(';');
        return new MotdInfo(true, strs);
    }
    catch (error) {
        //let er: Error = error
        return new MotdInfo(false, [error]);
    }
}
apis.Events.IM.OnGroupMessage.Add(e => {
    const { message } = e;
    if (message.startsWith("+") || message.startsWith("/")) {
        const list = e.messageMatch.getCommands(["+", "/"]);
        if (list.Count === 0)
            return;
        if (list[0].startsWith("motd")) {
            if (list.Count === 1) {
                e.feedback("参数不足,使用方法：/motd ip:port");
                return;
            }
            let port = 19132;
            if (list.Count === 3) {
                port = Number(list[2]);
            }
            let address = list[1];
            try {
                const li = address.lastIndexOf(":");
                if (li !== -1) {
                    const ps = Number(address.substr(li + 1));
                    if (ps > 0 && ps <= 65535) {
                        port = ps;
                        address = address.substring(0, li);
                    }
                }
            }
            catch (error) { }
            TaskRun(() => {
                const { success, detail, msg } = MotdBE(address, port);
                if (success) {
                    if (detail === undefined)
                        return;
                    const { description, platform, maxPlayer, protocolVersion, ipv4Port, ipv6Port, currentPlayer, gameMode, gameVersion, levelName } = detail;
                    e.feedback(`查询${address}:${port}成功\
\n类型：${platform}\
\n描述:${description}\
\n在线玩家：${currentPlayer}/${maxPlayer}\
\n游戏版本：${gameVersion}\
\n协议版本：${protocolVersion}\
\n游戏模式：${gameMode}\
\n存档名称：${levelName}`);
                }
                else {
                    e.feedback(`查询${address}:${port}失败:${msg}`);
                }
            });
        }
    }
});
