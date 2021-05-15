//本文件是脚本加载器，正常使用时请勿改动


/// <reference types="PFBridgeCore" />

//使用命名空间
var File = System.IO.File;
var Path = System.IO.Path;
var Directory = System.IO.Directory;
var FileSystem = importNamespace(Namespaces.Microsoft).VisualBasic.FileIO.FileSystem;
var core = importNamespace(Namespaces.Core)
var api = core.APIs.API
var MCConnections = core.ConnectionList.MCConnections


//#region 加载自定义程序集(位于libs目录下)
try {
    const AssemblyEx = core.AssemblyEx;
    const custom_libs_path = Path.Combine(api.PluginDataPath, "libs")//位于".\插件目录\libs\*.dll"
    if (!Directory.Exists(custom_libs_path)) Directory.CreateDirectory(custom_libs_path)//如果未找到目录就创建目录
    let FileList = FileSystem.GetFiles(custom_libs_path);
    for (let i = 0; i < FileList.Count; i++) {
        //let success = false;
        const file = FileList[i];
        try {
            if (!file.toLowerCase().endsWith(".dll")) continue;//跳过非dll文件
           /* success =*/ AssemblyEx.LoadFrom(file)
        } catch (e) {
            api.LogErr('加载自定义程序集"' + file + '"时遇到错误：' + e)
        }
        //if (success) { AssemblyEx.ReloadEngine(); }
    }
} catch (e) {
    api.LogErr("加载自定义程序集出错" + e)
}
//#endregion
const ResourceFiles = importNamespace(Namespaces.Core).My.Resources.ResourceFiles;
api.Log('JavaScript自定义配置加载中...');
api.Log('文件位于:' + Path.Combine(api.PluginDataPath, "index.js"));
//#region 清理重载前的残留
try {
    const events = core.APIs.Events
    events.IM.OnGroupMessage.Clear();
    events.MC.Chat.Clear();
    events.MC.Cmd.Clear();
    events.MC.Join.Clear();
    events.MC.Left.Clear();
} catch (e) { }
//#endregion
//#region 自定义脚本
const custom_script_path = Path.Combine(api.PluginDataPath, "scripts")
if (!Directory.Exists(custom_script_path)) {
    Directory.CreateDirectory(custom_script_path);
    api.Log('自定义脚本目录已创建:' + custom_script_path);
}
if (Directory.Exists(custom_script_path)) {
    //#region 如果没有脚本则输出默认自定义脚本
    if (FileSystem.GetFiles(custom_script_path).Count == 0) {
        File.WriteAllText(Path.Combine(custom_script_path, "main.js"), ResourceFiles.main);
        File.WriteAllText(Path.Combine(custom_script_path, "query.js"), ResourceFiles.query);
        File.WriteAllText(Path.Combine(custom_script_path, "command.js"), ResourceFiles.command);
        File.WriteAllText(Path.Combine(custom_script_path, "whitelist.js"), ResourceFiles.whitelist);
        File.WriteAllText(Path.Combine(custom_script_path, "format.js"), ResourceFiles.format);
        File.WriteAllText(Path.Combine(custom_script_path, "motd.js"), ResourceFiles.motd);
    }
    //#endregion
    let custom_script_success_count = 0, custom_script_failed_count = 0;
    let FileListAll = FileSystem.GetFiles(custom_script_path);//目录下所有文件
    let FileListJS = new Array();//加载列表
    for (var i = 0; i < FileListAll.Count; i++) {
        const file = FileListAll[i]
        if (file.toLowerCase().endsWith(".js")) FileListJS.push(file);//添加js文件到加载列表
    }
    api.Log('scripts目录下读取到' + FileListJS.length + '个自定义脚本，开始加载...');
    const engine = core.Main.Engine
    FileListJS.forEach(file => {
        try {
            let loadedInfo = engine.LoadModule(File.ReadAllText(file));
            const { Author, Description, Version } = loadedInfo
            api.Log('■■■■■■■■■■■■■■');
            api.Log('■脚本"' + Path.GetFileName(file) + '"加载成功!');
            api.Log('■作者：' + Author);
            let isFirstLine = true;
            Description.split("\n").forEach((s) => {
                api.Log((isFirstLine ? '■描述：' : '■　　　') + s);
                isFirstLine = false;
            });
            api.Log('■版本：' + Version);
            api.Log('■■■■■■■■■■■■■■');
            custom_script_success_count++;
        } catch (e) {
            api.LogErr('自定义脚本"' + Path.GetFileName(file) + '"运行出错：' + e.message);
            if (e.stack !== undefined) {
                api.LogErr("堆栈信息：");
                e.stack.split("\n").forEach((s: any) => {
                    api.LogErr(s);
                });
            }
            custom_script_failed_count++;
        }
    });
    if (custom_script_success_count > 0) api.Log(custom_script_success_count + '个脚本文件加载成功！');
    if (custom_script_failed_count > 0) api.LogErr(custom_script_failed_count + '个脚本文件加载失败！');
}
//#endregion
api.Log('JavaScript自定义配置已加载完毕！');