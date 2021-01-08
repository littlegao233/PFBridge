/*
//写入最新的默认index.js(用于更新)
 IO.File.WriteAllText(IO.Path.Combine(api.pluginDataPath, "index_new.js"), ResourceFiles.index)
*/
const FileSystem = importNamespace('Microsoft.VisualBasic.FileIO').FileSystem;
const IO = importNamespace('System.IO');
//#region 加载自定义程序集(位于libs目录下)
try {
    const custom_libs_path = IO.Path.Combine(api.pluginDataPath, "libs")//位于".\插件目录\libs\*.dll"
    if (!IO.Directory.Exists(custom_libs_path)) IO.Directory.CreateDirectory(custom_libs_path)//如果未找到目录就创建目录
    let FileList = FileSystem.GetFiles(custom_libs_path);
    for (let i = 0; i < FileList.Count; i++) {
        const file = FileList[i];
        if (!file.toLowerCase().endsWith(".dll")) continue;//跳过非dll文件
        try {
            if (AssemblyEx.LoadFrom(file)) { AssemblyEx.ReloadEngine(); }
        } catch (e) {
            api.LogErr('加载自定义程序集"' + file + '"时遇到错误：' + e)
        }
    }
} catch (e) {
    api.LogErr("加载自定义程序集出错" + e)
}
//#endregion
api.log('JavaScript自定义配置加载中...');
api.log('文件位于:' + IO.Path.Combine(api.pluginDataPath, "index.js"));
//#region 自定义脚本
const custom_script_path = IO.Path.Combine(api.pluginDataPath, "scripts")
if (!IO.Directory.Exists(custom_script_path)) {
    IO.Directory.CreateDirectory(custom_script_path);
    api.log('自定义脚本目录已创建:' + custom_script_path);
}
if (IO.Directory.Exists(custom_script_path)) {
    //#region 如果没有脚本则输出默认自定义脚本
    if (FileSystem.GetFiles(custom_script_path).Count == 0) IO.File.WriteAllText(IO.Path.Combine(custom_script_path, "main.js"), ResourceFiles.main)
    //#endregion
    let custom_script_success_count = 0, custom_script_failed_count = 0;
    let FileListAll = FileSystem.GetFiles(custom_script_path);//目录下所有文件
    let FileListJS = new Array();//加载列表
    for (var i = 0; i < FileListAll.Count; i++) {
        const file = FileListAll[i]
        if (file.toLowerCase().endsWith(".js")) FileListJS.push(file);//添加js文件到加载列表
    }
    api.log('scripts目录下读取到' + FileListJS.length + '个自定义脚本，开始加载...');
    FileListJS.forEach(file => {
        try {
            engine.Execute(IO.File.ReadAllText(file));
            api.log('自定义脚本"' + IO.Path.GetFileName(custom_script_path) + '"加载成功！');
            custom_script_success_count++;
        } catch (e) {
            api.LogErr('自定义脚本"' + IO.Path.GetFileName(custom_script_path) + '"运行出错：' + e);
            custom_script_failed_count++;
        }
    });
    if (custom_script_success_count > 0) api.log(custom_script_success_count + '个脚本文件加载成功！');
    if (custom_script_failed_count > 0) api.LogErr(custom_script_failed_count + '个脚本文件加载失败！');
}
//#endregion
api.log('JavaScript自定义配置已加载完毕！');


