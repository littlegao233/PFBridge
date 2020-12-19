const IO = System.IO;
api.Log('JavaScript自定义配置加载中...');
api.Log('文件位于:' + IO.Path.Combine(api.PluginDataPath, "index.js"));
//主体
events.OnGroupMessage.Add(function (e) {
    try {
        api.Log(JSON.stringify(e.Message));
    } catch (e) {
        api.Log("error:"+e);
    }
})


api.Log('JavaScript自定义配置已加载至文件末尾');