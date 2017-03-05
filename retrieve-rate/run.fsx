open System
#r "System.Xml.Linq"
open FSharp.Data
#r "Newtonsoft.Json"
open Newtonsoft.Json

#load @"D:\home\site\wwwroot\Domain.fs" 
open Domain

// sample XML message
[<Literal>]
let sampleRate = __SOURCE_DIRECTORY__ + "/sample.xml"
type Rate = XmlProvider<sampleRate>

let url = """http://query.yahooapis.com/v1/public/yql?q=select * from yahoo.finance.xchange where pair in ("GBPUSD")&env=store://datatables.org/alltableswithkeys"""

let getRate () = 
    let response = Rate.Load(url)
    response.Results.Rate.Rate

let Run(timer: TimerInfo, rateMessage: byref<string>, log: TraceWriter) =
    log.Info(
        sprintf "F# Timer trigger function executed at: %s" 
            (DateTime.Now.ToString()))

    let result = getRate ()
    let message = 
        { Rate = result } 
        |> JsonConvert.SerializeObject
        
    rateMessage <- message

    sprintf "%f" result            
    |> log.Info 