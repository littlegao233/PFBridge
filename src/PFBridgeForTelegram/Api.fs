module Api
open Telegram.Bot
open Telegram.Bot.Types
let mutable botClient:ITelegramBotClient=null
type ApiBase()=
    interface PFBridgeCore.IBridgeQQBase with
        member _this.Log(message: obj): unit = message|>Console.WriteLine
        member _this.LogErr(message: obj): unit = message|>Console.WriteLineWarn
        member val ParseMessageFormat =PFBridgeCore.Model.DefaultParseFormat():>PFBridgeCore.IParseMessageFormat with get, set
        member _this.PluginDataPath: string = 
            let p:string = System.IO.Path.GetFullPath("PFBridgeForTelegram");
            if p|>System.IO.Directory.Exists|>not then p|>System.IO.Directory.CreateDirectory|>ignore
            p
        member _this.SendGroupMessage(targetGroup: int64, message: string): unit = 
            botClient.SendTextMessageAsync(ChatId(targetGroup),message)|>ignore
        member _this.SendPrivateMessageFromGroup(targetGroup: int64, qQid: int64, message: string): unit = 
            let t=botClient.GetChatMemberAsync(ChatId(targetGroup),int qQid)
            t.Wait()
            botClient.SendTextMessageAsync(ChatId(t.Result.User.Id),message)|>ignore