namespace MagicTimes.Hub

open Countersoft.Gemini.Api
open Countersoft.Gemini.Commons.Entity
open Countersoft.Gemini.Commons.Dto

module HubModule = 
    open FSharp.Interop.Dynamic
    open Microsoft.AspNet.SignalR
    open Microsoft.AspNet.SignalR.Hubs
    open MagicTimes.Core
    open MagicTimes.Events
    open MagicTimes.Events.Event
    open MagicTimes.Events.Notification
    open MagicTimes.Events.Prompt
    
    let storePath = "nfc.datastore.path"
    let load() = storePath |> DataStore.load
    let save() = storePath |> DataStore.store
    
    let sendData() = 
        load()
        |> DataStore.convert
        |> create Types.RECIEVE_UPDATE
    
    let deleteSession (id) = 
        let p = load()
        let result = p.Value.Data.Remove(id)
        if result then 
            save () p |> printfn "DataStore: %A"
            success "Session is deleted" (sprintf "Session %A was deleted and data has been saved" id)
        else error "Session was not deleted" "Failed to delete session"
    
    let execute (x : Hub) command = 
        match command with
        | Command.LoadData -> sendData () x
        | Command.DeleteSession data -> deleteSession data.id x
        command
    
    let printConnection (x : Hub) command = printfn "%s: %s" command x.Context.ConnectionId
    let onConnect (x : Hub) = sendData () x
    
    type CommandHub() as this = 
        inherit Hub()
        member this.Command(data : Command.T) = execute this data
        
        override this.OnConnected() = 
            printConnection this "CONNECTED"
            onConnect this
            error "Poopidoo was not deleted" "Failed to delete session" this
            input "Who is this?" "New card not seen before" this
            base.OnConnected()
        
        override this.OnDisconnected stop = 
            printConnection this "DISCONNECTED"
            base.OnDisconnected stop
        
        override this.OnReconnected() = 
            printConnection this "RECONNECTED"
            onConnect this
            base.OnReconnected()
