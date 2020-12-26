const IO = System.IO;
api.log('JavaScript自定义配置加载中...');
//api.Log('文件位于:' + IO.Path.Combine(api.PluginDataPath, "index.js"));
api.log('文件位于:' + IO.Path.Combine(api.pluginDataPath, "index.js"));
try {
   throw
} catch (e) { api.log(e) }
 
/*
//写入最新的默认index.js(用于更新)
 IO.File.WriteAllText(IO.Path.Combine(api.pluginDataPath, "index_new.js"), ResourceFiles.index)
*/
//主体
events.onGroupMessage.add(function (e) {
    try {
        api.log(JSON.stringify(e));
        api.SendPrivateMessageFromGroup(e.fromGroup, e.fromQQ, "test:" + e.message)
        api.SendGroupMessage(e.fromGroup, "test1:" + e.message)
    } catch (e) {
        api.log("error:" + e);
    }
})


api.log('JavaScript自定义配置已加载至文件末尾');