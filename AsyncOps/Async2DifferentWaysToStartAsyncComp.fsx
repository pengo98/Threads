(* 
Different ways to start async computation and the order of execution. Compare execution using same UI (main)thread vs ThreadPool thread.
Whether the async computation will block or not depends on its async comp's behaviour (ie does it use any blocking calls inside Async ) and the way the async comp is started using one of the following calls:  

Async.Start
Async.StartImmediate
Async.RunSynchronously

*)
open System.Threading
open System.Threading.Tasks
let MyMethod1 = async {
    printfn "B%i" Thread.CurrentThread.ManagedThreadId
//    do! Async.AwaitTask (Tasks.Task.Delay 1000) // Same as Async.Sleep
//    let! result = Async.Sleep 1000
    do! Async.Sleep 1000 // Similar to above but just execute and discard the result
    printfn "C%i" Thread.CurrentThread.ManagedThreadId
}

let MyMethod2 = async {
    printfn "B%i" Thread.CurrentThread.ManagedThreadId
    Thread.Sleep 1000
    printfn "C%i" Thread.CurrentThread.ManagedThreadId  
}

let main(myMethod:Async<unit>, asyncstart) =
    printfn "A%i" Thread.CurrentThread.ManagedThreadId
    myMethod |> asyncstart  
    printfn "D%i" Thread.CurrentThread.ManagedThreadId

// Start async computation in thread pool. Do not await its result
// Results: A1 D1 B11 C10
main (MyMethod1, Async.Start)
// Run an async computation, starting immediate on the current os thread
// MyMethod1 does not block
// Results: A1 B1 D1 C1
main (MyMethod1, Async.StartImmediate) 
// Run an async computation, starting immediate on the current os thread
// MyMethod2 blocks as it uses Thread.Sleep as opposed to Async.Sleep to simulate long running task.
// Result: A1 B1 C1 D1
main (MyMethod2, Async.StartImmediate) 
// Runs async computation and await its result
// Result: A1 B12 C9 D1
main (MyMethod1, Async.RunSynchronously)
