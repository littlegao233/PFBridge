module EventHandler
open Telegram.Bot
open Telegram.Bot.Types
open PFBridgeCore

let OnGroupMessage(_:obj)(e:Args.MessageEventArgs)=
    Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}.")
    //Api.botClient.SendTextMessageAsync(chatId=new Types.ChatId(e.Message.Chat.Id),text="You said:\n" + e.Message.Text) |> ignore
    APIs.Events.IM.OnGroupMessage.Invoke(
        APIs.EventsMap.IMEventsMap.GroupMessageEventsArgs(
            e.Message.Chat.Id,
            int64 e.Message.From.Id,
            e.Message.Text, 
            (fun _->e.Message.Chat.Title),
            (fun _->e.Message.From.Username),
            (fun _->e.Message.Chat.Username),
            (fun _->1),//e.Message.Chat.Permissions.,
            (fun t->
            //Enums.ChatType.Group
                Api.botClient.SendTextMessageAsync(chatId=Types.ChatId(e.Message.Chat.Id),text=t) |> ignore
            ),
            (fun _->e.Message.Text)
        )
    )
