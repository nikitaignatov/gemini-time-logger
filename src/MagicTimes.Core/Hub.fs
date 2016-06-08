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
    
    let execute (x : Hub) command = 
        match command with
        | Command.LoadData -> 
            DataStore.Load "nfc.datastore.path"
            |> Option.toArray
            |> x.Clients.All?event
        command
    
    let printConnection (x : Hub) command = printfn "%s: %s" command x.Context.ConnectionId
    
    type CommandHub() as this = 
        inherit Hub()
        member this.Command(data : Command.T) = execute this data
        
        override this.OnConnected() = 
            printConnection this "CONNECTED"
            base.OnConnected()
        
        override this.OnDisconnected stop = 
            printConnection this "DISCONNECTED"
            base.OnDisconnected stop
        
        override this.OnReconnected() = 
            printConnection this "RECONNECTED"
            base.OnReconnected()
