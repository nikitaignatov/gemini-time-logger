namespace MagicTimes.Hub

open Countersoft.Gemini.Api
open Countersoft.Gemini.Commons.Entity
open Countersoft.Gemini.Commons.Dto
open System

module Session = 
    type SessionId = int
    
    type AddIssueId = 
        { id : SessionId
          issueId : int }
    
    type AddComment = 
        { id : SessionId
          comment : string }

module Settings = 
    type Settings = 
        { username : string
          apiKey : string }
    
    type ChangeSettings = 
        { Settings : Settings }

module Command = 
    type T = 
        | ChangeSettings of Settings.ChangeSettings
        | AddIssueId of Session.AddIssueId
        | AddComment of Session.AddComment
        | ResetSettings
        | LoadData

module HubModule = 
    open FSharp.Interop.Dynamic
    open Microsoft.AspNet.SignalR
    open Microsoft.AspNet.SignalR.Hubs
    open MagicTimes.Core
    
    type Event<'a> = 
        { ``type`` : string
          data : 'a }
    
    let wrap (name : string) (data : Option<'a>) = 
        { ``type`` = name.ToUpper()
          data = Option.toArray data }
    
    let event name (data : Option<'a>) (x : Hub) = wrap name data |> x.Clients.All?event
    
    let sendData = 
        "nfc.datastore.path"
        |> DataStore.Load
        |> DataStore.convert
        |> event "RECIEVE_UPDATE"
    
    let execute (x : Hub) command = 
        match command with
        | Command.LoadData -> sendData x
        command
    
    let printConnection (x : Hub) command = printfn "%s: %s" command x.Context.ConnectionId
    let onConnect (x : Hub) = sendData x
    
    type CommandHub() as this = 
        inherit Hub()
        member this.Command(data : Command.T) = execute this data
        
        override this.OnConnected() = 
            printConnection this "CONNECTED"
            onConnect this
            base.OnConnected()
        
        override this.OnDisconnected stop = 
            printConnection this "DISCONNECTED"
            base.OnDisconnected stop
        
        override this.OnReconnected() = 
            printConnection this "RECONNECTED"
            onConnect this
            base.OnReconnected()
