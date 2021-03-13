module Config
open System.IO
open System
open Newtonsoft.Json
type ConfigModel()=
    member val Token="" with get, set
    //member _.M(x: Nullable<ConfigModel>) = x.HasValue
    //member val NVT = Nullable 12 with get, set
let ConfigPath=Path.GetFullPath("config.json")
let mutable _configData=ConfigModel()
let mutable setup=false
let ConfigData:ConfigModel=
    if setup then _configData else
        let data=
            if ConfigPath|>File.Exists then
                File.ReadAllText(ConfigPath)
            else
                Console.WriteLine $"writing default config file into {ConfigPath}"
                _configData|>JsonConvert.SerializeObject
        _configData<-data|>JsonConvert.DeserializeObject<ConfigModel>
        (ConfigPath,_configData|>JsonConvert.SerializeObject)|>File.WriteAllText
        setup<-true
        _configData
        



