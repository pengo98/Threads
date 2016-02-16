(*
Starting And Waiting For Task<T> In A More F# Like Way

The Async class offers a couple of helpers when dealing with tasks, you may use

Async.StartAsTask
Async.AwaitTask
*)

open System
open System.Threading
open System.Threading.Tasks


let work () = async {
    for i in 0 .. 2 do
        printfn "Work loop is currently %O" i |> ignore
        do! Async.Sleep 1000
    
    return "Task is completed " + DateTime.Now.ToLongTimeString() 
}
  
printfn "Starting task that returns a value" |> ignore

let asyncWorkFlow = async {
    //NOTE : Async.StartAsNewTask doesn't like TaskCreationOptions.LongRunning
//    let task = Async.StartAsTask(work(), TaskCreationOptions.LongRunning)
    let task = Async.StartAsTask(work())
    let! result = Async.AwaitTask task 
    return result
}

let finalResult = Async.RunSynchronously asyncWorkFlow
printfn "Task result is : %O" finalResult
