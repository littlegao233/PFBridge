module Console
let PluginName="PFBridge"
let WriteLineT(subtype:obj)(content :obj)=
    printfn "\x1b[38;2;176;196;222m[\x1b[38;2;169;169;169mINFO\x1b[38;2;176;196;222m][\x1b[38;2;167;132;239m%s\x1b[38;2;176;196;222m]\x1b[38;2;0;255;127m|\x1b[38;2;255;0;255m%s\x1b[38;2;0;255;127m|\x1b[38;2;250;250;210m%s\x1b[0m" 
        PluginName 
        (content.ToString()) 
        (subtype.ToString())
let WriteLine(content :obj)=
    printfn "\x1b[38;2;176;196;222m[\x1b[38;2;169;169;169mINFO\x1b[38;2;176;196;222m][\x1b[38;2;167;132;239m%s\x1b[38;2;176;196;222m]\x1b[38;2;250;250;210m%s\x1b[0m"
         PluginName
         (content.ToString())
let WriteLineWarnT(content:obj)(tip:string)=
    printfn "\x1b[38;2;240;128;128m[\x1b[38;2;253;99;71mWARN\x1b[38;2;240;128;128m][\x1b[38;2;167;132;239m%s\x1b[38;2;240;128;128m]\x1b[38;2;220;138;138m%s\x1b[38;2;152;201;120m-->\x1b[4m\x1b[38;2;127;255;1m\x1b[48;2;25;25;112m%s\x1b[0m"
        PluginName
        (content.ToString())
        (tip.ToString())
let WriteLineWarn(content:obj)=
    printfn "\x1b[38;2;240;128;128m[\x1b[38;2;253;99;71mWARN\x1b[38;2;240;128;128m][\x1b[38;2;167;132;239m%s\x1b[38;2;240;128;128m]\x1b[38;2;220;138;138m%s\x1b[38;2;152;201;120m\x1b[0m"
        PluginName
        (content.ToString())
let WriteLineErr(content:obj)(ex:exn)=
    printfn "\x1b[93m\x1b[41m[\x1b[0m\x1b[101m\x1b[4mERROR\x1b[0m\x1b[93m\x1b[41m]\x1b[0m\x1b[38;2;138;143;226m[\x1b[38;2;167;132;239m%s\x1b[38;2;138;143;226m]\x1b[38;2;234;47;39m%s\n\x1b[38;2;147;147;119m%s\x1b[0m"
        PluginName
        (content.ToString())
        (ex.ToString())
let ColorFg(color:System.Drawing.Color)= $"\x1b[38;2;{color.R};{color.G};{color.B}m"
let ColorReset="\x1b[0m"