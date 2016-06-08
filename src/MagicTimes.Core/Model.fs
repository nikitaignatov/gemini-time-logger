namespace MagicTimes.Core
module Model = 
    open System
    open System.Collections.Generic
    open Gemini.Commander.Nfc
    
    type WorkingHours = 
        { date : DateTime
          started : double
          ended : double
          lunch : bool
          confirmed : bool }
    
    type Gemini = 
        { api_key : string
          ticket_url : string }
    
    type Settings = 
        { gemini : Gemini
          round_minutes_to : int }
    
    type User = 
        { Id : int
          Username : string
          Name : string
          Email : string
          Created : DateTime
          Cards : string list }
    
    type TimeType = 
        | None = 0
        | Question = 1
        | Consulting = 2
        | Discussion = 3
        | Status = 4
        | Meeting = 5
        | NonWorkRelated = 6
    
    type TrackerSession = 
        { TimeEntryId : string
          SessionId : Guid
          Name : string
          Transaction : CardTransaction
          Transactions : CardTransaction list
          Attributes : Dictionary<string, Object>
          Ticket : string
          Type : TimeType
          Message : string }
        member this.IsMissingTimeEntryId = String.IsNullOrWhiteSpace(this.TimeEntryId) || this.TimeEntryId = "0"
        member this.IsSubmitted = not this.IsMissingTimeEntryId
        member this.IsMissingTicket = String.IsNullOrWhiteSpace(this.Ticket)
        member this.IsMissingMessage = String.IsNullOrWhiteSpace(this.Message)
        member this.IsValid = 
            [| this.IsMissingTimeEntryId; this.IsMissingTicket; this.IsMissingMessage |] 
            |> Array.forall (fun c -> not c)
    
    let gemini key url = 
        { api_key = key
          ticket_url = url }
    
    let settings gemini rounding = 
        { gemini = gemini
          round_minutes_to = rounding }
    
    let defaultSettings = (settings (gemini "SOME_KEY" "http://gemini.company.com/workspace/0/item/{ticket}") 15)
