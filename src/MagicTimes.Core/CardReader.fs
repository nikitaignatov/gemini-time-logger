namespace MagicTimes.Core

// TODO refactor this module
module CardReader = 
    open MagicTimes
    open MagicTimes.Core
    open FSharp.Interop.Dynamic
    open Microsoft.AspNet.SignalR
    open Microsoft.AspNet.SignalR.Hubs
    open MagicTimes.Core.Model
    open System
    open System.Linq
    open Gemini.Commander.Nfc
    
    type Register = 
        { card : string
          users : Model.User list }
    
    let withinLast minutes = fun tx -> (DateTime.Now - tx.Transaction.Started).TotalMinutes < minutes
    
    let existing (x : CardTransaction) (store : DataStore.T) = 
        let values = Map.toSeq >> Seq.map snd
        store.Data
        |> values
        |> Seq.filter DataStore.notSubmitted
        |> Seq.filter (fun c -> 
               c.Transactions
               |> Seq.map (fun m -> m.Card)
               |> DataStore.withCard x.Card)
        |> Seq.tryFind (withinLast (float 15))
    
    let change x store (session : TrackerSession) ctx = 
        let e = existing x store
        
        let data = 
            match existing x store with
            | Some e -> 
                let session2 = { session with Name = session.Name + ", " + store.CardUsers().[x.Card].Name }
                let e2 = { e with Transactions = x :: e.Transactions }
                store.Data.Remove(x.TransactionId).Add(x.TransactionId, e2)
            | None -> store.Data.Add(x.TransactionId, session)
        
        let message = DataStore.store (Some { store with Data = data })
        Events.Notification.info "DATA STORE" message ctx
        Hub.HubModule.sendData () ctx
    
    let newSession x (store : DataStore.T) = Model.create x (store.CardUsers().[x.Card].Name)
    let context = GlobalHost.ConnectionManager.GetHubContext<Hub.HubModule.CommandHub>()
    
    let insert (store : DataStore.T) x = 
        let session = newSession x store
        let newCard = not (store.CardUsers().ContainsKey(x.Card))
        if newCard then Events.Prompt.input "Who is this?" "Assign a user to this card" context.Clients.All
        else change x store session context.Clients.All
        x
    
    let update (store : DataStore.T) (x : CardTransaction) = 
        if (store.CardUsers().ContainsKey(x.Card) && store.Data.ContainsKey(x.TransactionId)) then 
            let session = Model.addTransaction store.Data.[x.TransactionId] x
            let data = store.Data.Remove(x.TransactionId).Add(x.TransactionId, session)
            let message = DataStore.store (Some { store with Data = data })
            Events.Notification.info "DATA STORE" message context.Clients.All
            Hub.HubModule.sendData () context.Clients.All
    
    let execute store reader = 
        match store with
        | Some store -> Reader.start (insert store) (update store) reader
        | None -> ()
