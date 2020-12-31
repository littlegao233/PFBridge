/*
//写入最新的默认index.js(用于更新)
 IO.File.WriteAllText(IO.Path.Combine(api.pluginDataPath, "index_new.js"), ResourceFiles.index)
*/
const FileSystem = importNamespace('Microsoft.VisualBasic.FileIO').FileSystem;
const IO = importNamespace('System.IO');

api.log('JavaScript自定义配置加载中...');
api.log('文件位于:' + IO.Path.Combine(api.pluginDataPath, "index.js"));
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
AddWebsocket("ws://echo.websocket.org", "")

//主体部分
events.onGroupMessage.add(function (e) {
    try {
        api.log(JSON.stringify(e));
        api.SendPrivateMessageFromGroup(e.fromGroup, e.fromQQ, "test:" + e.message)
        api.SendGroupMessage(e.fromGroup, "test1:" + e.message)
    } catch (e) {
        api.log("error:" + e);
    }
})




//#region 自定义脚本

const custom_script_path = IO.Path.Combine(api.pluginDataPath, "scripts")
if (!IO.Directory.Exists(custom_script_path)) {
    IO.Directory.CreateDirectory(custom_script_path);
    api.log('自定义脚本目录已创建:' + custom_script_path);
}
if (IO.Directory.Exists(custom_script_path)) {
    let custom_script_success_count = 0, custom_script_failed_count = 0;
    let FileList = FileSystem.GetFiles(custom_script_path);
    api.log('scripts目录下读取到' + FileList.Count + '个自定义脚本，开始加载...');
    for (let i = 0; i < FileList.Count; i++) {
        const file = FileList[i];
        try {
            engine.Execute(IO.File.ReadAllText(file));
            api.log('自定义脚本"' + IO.Path.GetFileName(custom_script_path) + '"加载成功！');
            custom_script_success_count++;
        } catch (e) {
            api.LogErr('自定义脚本"' + IO.Path.GetFileName(custom_script_path) + '"运行出错：' + e);
            custom_script_failed_count++;
        }
    }
    if (custom_script_success_count > 0) api.log(custom_script_success_count + '个脚本文件加载成功！');
    if (custom_script_failed_count > 0) api.LogErr(custom_script_failed_count + '个脚本文件加载失败！');
}
//#endregion

api.log('JavaScript自定义配置已加载完毕！');