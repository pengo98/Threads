open System
open System.Threading

let Go () =
    failwith "Exception"

try
    let thread = new Thread(Go)
    thread.Start()
with
    | ex -> printfn "%s" ex.Message 
