// Learn more about F# at http://fsharp.org

open System
open Telegram.Bot
open Newtonsoft.Json.Linq

[<EntryPoint>]
let main _ =
    try
        Api.botClient <- new TelegramBotClient(Config.ConfigData.Token)
    with 
    | :? System.ArgumentException as ex->
        Console.WriteLineErr "ArgumentException" (ex)
        Console.WriteLine "Please edit config.json to correct Token."
        Console.WriteLine "Press any key to exit."
        Console.ReadKey()|>ignore
        exit(0)
    let mutable login:Threading.Tasks.Task<Types.User>=null
    Threading.Tasks.Task.Run(fun ()->
        while true do
            try
                login<-Api.botClient.GetMeAsync()
            with ex->
                Console.WriteLineErr $"Unable to connect to Telegram server." ex
                Threading.Thread.Sleep(3000)
            try
                Console.WriteLine($"Connecting to Telegram server.")
                let me = login.Result
                Console.WriteLine($"Login Success.")
                Console.WriteLine($"Bot User ID : {me.Id}")
                Console.WriteLine($"Bot User Name : {me.FirstName}")
                Api.botClient.OnMessage.AddHandler(EventHandler<Args.MessageEventArgs>(EventHandler.OnGroupMessage))
                Api.botClient.StartReceiving()
                while true do
                    Api.botClient.TestApiAsync()|>ignore
                    Threading.Thread.Sleep(10000)
            with ex->
                Console.WriteLineErr $"Connection lost." ex
                Threading.Thread.Sleep(3000)
    )
    |>ignore
    let mutable status=true
    let split= $"{Console.ColorReset}{Console.ColorFg(Drawing.Color.Gray)} - \x1b[3m"
    while status do
        match Console.ReadLine() with
        |"help"->
            ("\n"+Console.ColorFg(Drawing.Color.Orange)+"\x1b[4m\t\t",[|
            "All available command :"
            $"""{"help"+"\x1b[0m",-15}{split}Display this message"""
            $"""{"stop"+"\x1b[0m",-15}{split}Offline the Bot and exit this app"""
            $"""{"status"+"\x1b[0m",-15}{split}Query the status of the Bot"""
            |])
            |>String.Join|>Console.WriteLine
        |"stop"|"exit"->status<-false
        |"status"->
            let mutable all=[Collections.Generic.KeyValuePair<string,JToken>("Status",JValue(if Api.botClient.IsReceiving then "Receiving" else "Offline"))]
            if login|>isNull|>not then
                if login.IsCompletedSuccessfully then
                    for node in JObject.FromObject(login.Result) do all<-node::all
            for node in all do
                let key= $"""{node.Key.Replace("_"," "),10}"""
                Console.WriteLine($"""{key,-28}= {node.Value}""")
                //Console.WriteLine($"LanguageCode\t:\t{login.Result.LanguageCode}")
        |cmd-> 
            $"Unknown command {cmd}.Please use command 'help' to check all command."
            |>Console.WriteLineWarn
    Console.WriteLine($"Stopping...")
    try if login|>isNull|>not then if login.IsCompletedSuccessfully then Api.botClient.StopReceiving()
    with | :? System.NullReferenceException->()
    0 // return an integer exit code