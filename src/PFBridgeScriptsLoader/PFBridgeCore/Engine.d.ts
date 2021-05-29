/// <reference path="./Utils.d.ts" />
/// <reference path="./APIs.d.ts" />
/// <reference path="./Microsoft.d.ts" />
declare function importNamespace(name: "PFBridgeCore.Utils.Net.Sockets" | Namespaces.UtilsNetSockets): UtilsNetSockets
declare function importNamespace(name: "PFBridgeCore" | Namespaces.Core): IPFBridgeCore
declare function importNamespace(name: "Microsoft.VisualBasic.FileIO" | Namespaces.MicrosoftVisualBasicFileIO): MicrosoftVisualBasicFileIO
declare function importNamespace(name: "System.IO"): typeof System.IO
declare function importNamespace(name: 'System.Threading.Tasks'): typeof System.Threading.Tasks 
/**
 * 导入.Net命名空间
 * @param name 命名空间
 */
declare function importNamespace(name: string): any
declare const enum Namespaces {
    Core = "PFBridgeCore",
    CoreResources = "PFBridgeCore.My.Resources",
    UtilsNetSockets = "PFBridgeCore.Utils.Net.Sockets",
    MicrosoftVisualBasicFileIO = "Microsoft.VisualBasic.FileIO"
}
declare interface IPFBridgeCore {
    Utils: Utils;
    APIs: PFBridgeCoreAPIs;
    ConnectionList: {
        MCConnections: System.Collections.Generic.List<IBridgeMCBase>
    };
    ConnectionManager: {
        AddWebsocketClient(url: string, token: string): Integer
        AddWebsocketClient(url: string, token: string, tag: any): Integer
    }
    AssemblyEx: {
        LoadFrom(path: string): boolean
    }
    My: {
        Resources: { ResourceFiles: any }
    }
    Main: {
        Engine: {
            LoadModule(path: string): {
                Author: string | "unknown";
                Description: string | "unknown";
                Version: string | "unknown";
            }
            SetShareData(key: string, value: any)
            GetShareData(key: string): {
                Key: string
                Value: any
            }
        }
    }
}