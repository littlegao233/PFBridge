//#region >>>>>-----公共方法----->>>>>
/**
 * 添加基于WebsocketAPI的mc连接
 * @param {string} url websocket地址
 * 格式：ws://地址:端口/终端
 * 参考：ws://127.0.0.1:29132/mcws
 * @param {string} token 密匙串（用于运行命令等操作）
 */
function AddWebsocket(url, token) { ConnectionManager.AddWebsocketClient(url, token) }
//#endregion <<<<<-----公共方法-----<<<<<

//主体部分
AddWebsocket("ws://echo.websocket.org", "")

events.onGroupMessage.add(function (e) {
    try {
        api.log(JSON.stringify(e));
        api.SendPrivateMessageFromGroup(e.fromGroup, e.fromQQ, "test:" + e.message)
        api.SendGroupMessage(e.fromGroup, "test1:" + e.message)
    } catch (e) {
        api.log("error:" + e);
    }
})

