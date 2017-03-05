open System.Text
#r "System.Net.Http"
open System.Net.Http
#r "Newtonsoft.Json"
open Newtonsoft.Json
open System.Configuration

#load @"D:\home\site\wwwroot\Domain.fs" 
open Domain

let Run(newRateMessage: string, log: TraceWriter) =

    log.Info(sprintf "F# Queue trigger function processing: '%s'" newRateMessage)
    
    let message = 
        JsonConvert.DeserializeObject<Message>(newRateMessage)

    let slackMessage = 
        message.Rate
        |> sprintf """{"text":"Rate : %f"}""" 

    let client = new HttpClient()
    let message = new StringContent(slackMessage, Encoding.UTF8)
    let appSettings = ConfigurationManager.AppSettings  
    let url = appSettings.["SlackURL"]
    
    client.PostAsync(url,message) |> ignore

    log.Info(sprintf "F# Queue trigger function processed: '%s'" newRateMessage)
