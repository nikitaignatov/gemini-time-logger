namespace MagicTimes.Core

module Serializer = 
    open System
    open System.Configuration
    open System.IO
    open Newtonsoft.Json
    
    let setting (key : string) = 
        let path = ConfigurationManager.AppSettings.[key]
        if String.IsNullOrWhiteSpace(path) then None
        else if not (File.Exists path) then None
        else Some path
    
    let deserialize<'a> (json : string) = JsonConvert.DeserializeObject<'a> json
    
    let deserializeFile<'a> = 
        File.ReadAllText
        >> deserialize<'a>
        >> Some
    
    let serialize (o) = JsonConvert.SerializeObject(o, Formatting.Indented)
    let serializeFile path o = File.WriteAllText(path, serialize o)

module DataStore = 
    open Model
    open System.Collections.Generic
    open Serializer
    open System
    open System.Linq
    
    type T = 
        { Data : Dictionary<Guid, TrackerSession>
          SessionUser : Dictionary<Guid, int>
          UserStore : Dictionary<string, User>
          Settings : Settings
          WorkingHours : WorkingHours list }
    
    let create = 
        { Data = new Dictionary<Guid, TrackerSession>()
          SessionUser = new Dictionary<Guid, int>()
          UserStore = new Dictionary<string, User>()
          Settings = defaultSettings
          WorkingHours = List.empty }
    
    let Load(key : string) = 
        match setting key with
        | Some path -> deserializeFile<T> path
        | None -> None
    
    let Store (key : string) (input : Option<T>) = 
        match setting key, input with
        | Some path, Some data -> 
            serializeFile path data
            sprintf "Data stored in %s." path
        | _ -> "Failed to store data."
    
    let convert (input : Option<T>) = 
        let inline (=>) a b = a, box b
        match input with
        | Some store -> 
            let data = store.Data.OrderByDescending(fun v -> v.Value.Transaction.Started)
            printfn "%A" data
            dict [ "users" => store.UserStore.Select(fun x -> x.Value).ToArray()
                   "new_sessions" => data.Where(fun x -> not x.Value.IsValid).ToArray()
                   "ready_to_submit" => data.Where(fun x -> x.Value.IsValid && not x.Value.IsSubmitted).ToArray()
                   "complete" => data.Where(fun x -> x.Value.IsSubmitted).ToArray()
                   "total_minutes" => data.Where(fun x -> x.Value.Transaction.IsEnded).Sum(fun x -> x.Value.Transaction.Duration.TotalMinutes)
                   "total_sessions" => data.Count()
                   "total_questions" => data.Count(fun x -> x.Value.Type = Model.TimeType.Question)
                   "settings" => store.Settings
                   "DataStore" => store ]
            |> Some
        | _ -> None
