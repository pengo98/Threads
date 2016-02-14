open System
open System.Threading

let Go () =
    printfn "hello from %s " Thread.CurrentThread.Name

Thread.CurrentThread.Name <- "main"
let worker = new Thread(Go)
worker.Name <- "worker"
worker.Start()

Go ()

