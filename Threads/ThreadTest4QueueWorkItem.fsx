open System
open System.Threading

let synRoot = ref 0
let presenterFunction() =
    let id = Thread.CurrentThread.ManagedThreadId
    printfn "I (%d) starting, please do not interrupt!" id
    Thread.Sleep 1000
    printfn "I (%d) finished." id


let longTalk() =
    lock(synRoot) (fun() -> presenterFunction())

ThreadPool.QueueUserWorkItem(fun _ -> longTalk())
ThreadPool.QueueUserWorkItem(fun _ -> longTalk())
ThreadPool.QueueUserWorkItem(fun _ -> longTalk())

