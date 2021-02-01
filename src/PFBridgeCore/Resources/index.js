//本文件是脚本加载器，正常使用时请勿改动
/*
//写入最新的默认index.js(用于更新)
 IO.File.WriteAllText(IO.Path.Combine(api.pluginDataPath, "index_new.js"), ResourceFiles.index)
*/
const IO = importNamespace('System.IO');//导入命名空间
const FileSystem = importNamespace('Microsoft.VisualBasic.FileIO').FileSystem;
const PFBridgeCore = importNamespace('PFBridgeCore')
const api = PFBridgeCore.APIs.API
const events = PFBridgeCore.APIs.Events
const MCConnections = PFBridgeCore.ConnectionList.MCConnections
//#region 加载自定义程序集(位于libs目录下)
try {
    const AssemblyEx = PFBridgeCore.AssemblyEx;
    const custom_libs_path = IO.Path.Combine(api.pluginDataPath, "libs")//位于".\插件目录\libs\*.dll"
    if (!IO.Directory.Exists(custom_libs_path)) IO.Directory.CreateDirectory(custom_libs_path)//如果未找到目录就创建目录
    let FileList = FileSystem.GetFiles(custom_libs_path);
    for (let i = 0; i < FileList.Count; i++) {
        let success = false;
        try {
            const file = FileList[i];
            if (!file.toLowerCase().endsWith(".dll")) continue;//跳过非dll文件
            success = AssemblyEx.LoadFrom(file)
        } catch (e) {
            api.LogErr('加载自定义程序集"' + file + '"时遇到错误：' + e)
        }
        if (success) { AssemblyEx.ReloadEngine(); }
    }
} catch (e) {
    api.LogErr("加载自定义程序集出错" + e)
}
//#endregion
const ResourceFiles = importNamespace('PFBridgeCore.My.Resources').ResourceFiles;
api.log('JavaScript自定义配置加载中...');
api.log('文件位于:' + IO.Path.Combine(api.pluginDataPath, "index.js"));
//#region 清理重载前的残留
try {
    const events = PFBridgeCore.APIs.Events
    events.QQ.OnGroupMessage.Clear();
    events.Server.Chat.Clear();
    events.Server.Cmd.Clear();
    events.Server.Join.Clear();
    events.Server.Left.Clear();
} catch (e) { }
//#endregion
//#region 自定义脚本
const custom_script_path = IO.Path.Combine(api.pluginDataPath, "scripts")
if (!IO.Directory.Exists(custom_script_path)) {
    IO.Directory.CreateDirectory(custom_script_path);
    api.log('自定义脚本目录已创建:' + custom_script_path);
}
if (IO.Directory.Exists(custom_script_path)) {
    //#region 如果没有脚本则输出默认自定义脚本
    if (FileSystem.GetFiles(custom_script_path).Count == 0) {
        IO.File.WriteAllText(IO.Path.Combine(custom_script_path, "main.js"), ResourceFiles.main);
        IO.File.WriteAllText(IO.Path.Combine(custom_script_path, "query.js"), ResourceFiles.query);
        IO.File.WriteAllText(IO.Path.Combine(custom_script_path, "command.js"), ResourceFiles.command);
    }
    //#endregion
    let custom_script_success_count = 0, custom_script_failed_count = 0;
    let FileListAll = FileSystem.GetFiles(custom_script_path);//目录下所有文件
    let FileListJS = new Array();//加载列表
    for (var i = 0; i < FileListAll.Count; i++) {
        const file = FileListAll[i]
        if (file.toLowerCase().endsWith(".js")) FileListJS.push(file);//添加js文件到加载列表
    }
    api.log('scripts目录下读取到' + FileListJS.length + '个自定义脚本，开始加载...');
    const engine = PFBridgeCore.Main.Engine
    FileListJS.forEach(file => {
        try {
            let loadedInfo = engine.LoadModule(IO.File.ReadAllText(file));
            const { Author, Description, Version } = loadedInfo
            api.log('██████████████');
            api.log('█脚本"' + IO.Path.GetFileName(file) + '"加载成功!');
            api.log('█作者：' + Author);
            let isFirstLine = true;
            Description.split("\n").forEach((s) => {
                api.log((isFirstLine ? '█描述：' : '█　　　') + s);
                isFirstLine = false;
            });
            api.log('█版本：' + Version);
            api.log('██████████████');
            custom_script_success_count++;
        } catch (e) {
            api.LogErr('自定义脚本"' + IO.Path.GetFileName(file) + '"运行出错：' + e);
            custom_script_failed_count++;
        }
    });
    if (custom_script_success_count > 0) api.log(custom_script_success_count + '个脚本文件加载成功！');
    if (custom_script_failed_count > 0) api.LogErr(custom_script_failed_count + '个脚本文件加载失败！');
}
//#endregion
api.log('JavaScript自定义配置已加载完毕！');


