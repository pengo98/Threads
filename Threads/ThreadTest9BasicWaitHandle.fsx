open System.Threading

let waitHandle = new AutoResetEvent(false)

let waiter() =
    printfn "Waiting..."
    waitHandle.WaitOne() |> ignore
    printfn "Notified"

(new Thread(waiter)).Start()
Thread.Sleep 1000
waitHandle.Set()

