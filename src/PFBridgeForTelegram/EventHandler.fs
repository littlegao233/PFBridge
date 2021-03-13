module EventHandler
open Telegram.Bot
open Telegram.Bot.Types
let OnGroupMessage(_:obj)(e:Args.MessageEventArgs)=
    Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}.")
    Api.botClient.SendTextMessageAsync(chatId=new Types.ChatId(e.Message.Chat.Id),text="You said:\n" + e.Message.Text) |> ignore
