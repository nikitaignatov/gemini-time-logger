namespace App

module Starter = 
    open Owin
    open Microsoft.Owin.Hosting
    open Microsoft.Owin.Cors
    open System
    
    [<EntryPoint>]
    let main argv = 
        let startup (a : IAppBuilder) = 
            a.UseCors(CorsOptions.AllowAll) |> ignore
            a.MapSignalR() |> ignore
        
        let hostUrl = "http://localhost:8085"
        use app = WebApp.Start(hostUrl, startup)
        Console.WriteLine("Server running on " + hostUrl)
        Console.ReadLine() |> ignore
        0
