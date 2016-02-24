open System.Threading

let waitHandle = new AutoResetEvent(false)

let waiter() =
    printfn "Waiting1..."
    waitHandle.WaitOne() |> ignore
    printfn "Notified2"
    waitHandle.WaitOne() |> ignore
    printfn "Notified3"
    waitHandle.WaitOne() |> ignore
    printfn "Notified4"

let waiter2() =
    printfn "Waiting5..."
    waitHandle.WaitOne() |> ignore
    printfn "Notified6"
    waitHandle.WaitOne() |> ignore
    printfn "Notified7"
    waitHandle.WaitOne() |> ignore
    printfn "Notified8"

(new Thread(waiter)).Start()
(new Thread(waiter2)).Start()

Thread.Sleep 1000
waitHandle.Set()

