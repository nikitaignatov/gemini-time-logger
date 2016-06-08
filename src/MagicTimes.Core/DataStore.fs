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
