namespace MagicTimes.Core

open System

module Session = 
    type SessionId = Guid
    
    type AddIssueId = 
        { id : SessionId
          issueId : int }
    type DeleteSession = 
        { id : Guid }
    
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
        | DeleteSession of Session.DeleteSession
        | ResetSettings
        | LoadData
