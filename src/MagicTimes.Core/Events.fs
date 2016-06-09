namespace MagicTimes.Events

module Types = 
    //
    let RECIEVE_UPDATE = "RECIEVE_UPDATE"
    let SESSION_DELETE = "SESSION_DELETE"
    let SESSION_DELETE_COMPLETE = "SESSION_DELETE_COMPLETE"
    let SESSION_DELETE_FAILED = "SESSION_DELETE_FAILED"
    let SESSION_UPDATE = "SESSION_UPDATE"
    let SESSION_SUBMIT_TIME = "SESSION_SUBMIT_TIME"
    let SESSION_SUBMIT_TIME_COMPLETE = "SESSION_SUBMIT_TIME_COMPLETE"
    //
    let USERS_IMPORT_SUCCESS = "USERS_IMPORT_SUCCESS"
    let USERS_REGISTER_SUCCESS = "USERS_REGISTER_SUCCESS"
    //
    let NEW_CARD_RECIEVED = "NEW_CARD_RECIEVED"
    //
    let RECIEVE_SESSIONS = "RECIEVE_SESSIONS"
    let CHANGE_SETTINGS = "CHANGE_SETTINGS"
    let CHANGE_SETTINGS_COMPLETE = "CHANGE_SETTINGS_COMPLETE"
    //
    let SERVER_ERROR_LOG = "SERVER_ERROR_LOG"
    //
    let NOTIFICATION_ADDED = "NOTIFICATION_ADDED"
    let NOTIFICATION_REMOVED = "NOTIFICATION_REMOVED"

module Event = 
    open FSharp.Interop.Dynamic
    open Microsoft.AspNet.SignalR
    open Microsoft.AspNet.SignalR.Hubs
    
    type T<'a> = 
        { ``type`` : string
          data : 'a }
    
    let wrap (name : string) (data : Option<'a>) = 
        { ``type`` = name.ToUpper()
          data = Option.toArray data }
    
    let create<'a> name (data : Option<'a>) (x : Hub) = 
        wrap name data |> x.Clients.All?event
        ()

module Notification = 
    type Kind = 
        | Success
        | Info
        | Error
        | Warning
    
    type T = 
        { title : string
          message : string
          kind : Kind }
    
    let notify kind title message = 
        { title = title
          message = message
          kind = kind }
        |> Some
        |> Event.create Types.NOTIFICATION_ADDED
    
    let success<'a> = notify Kind.Success
    let info<'a> = notify Kind.Info
    let warning<'a> = notify Kind.Warning
    let error<'a> = notify Kind.Error

module Prompt = 
    type Kind = 
        | Text
        | YesNo
        | Select
    
    type T = 
        { title : string
          message : string
          kind : Kind }
    
    let prompt kind title message = 
        { title = title
          message = message
          kind = kind }
        |> Some
        |> Event.create Types.NEW_CARD_RECIEVED
    
    let input<'a> = prompt Kind.Text
    let yesNo<'a> = prompt Kind.YesNo
    let select<'a> = prompt Kind.Select
