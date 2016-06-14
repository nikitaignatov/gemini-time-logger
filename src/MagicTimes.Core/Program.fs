namespace App

module Starter = 
    open Owin
    open Microsoft.Owin.Hosting
    open Microsoft.Owin.Cors
    open System
    open MagicTimes
    open MagicTimes.Core
    open Gemini.Commander.Nfc
    
    [<EntryPoint>]
    let main argv = 
        let startup (a : IAppBuilder) = 
            a.UseCors(CorsOptions.AllowAll) |> ignore
            a.MapSignalR() |> ignore
        
        let hostUrl = "http://localhost:8085"
        use app = WebApp.Start(hostUrl, startup)
        Console.WriteLine("Server running on " + hostUrl)
        let r = new MockReader()
        CardReader.execute (DataStore.load()) (new MockReader())
        Console.ReadLine() |> ignore
        0
