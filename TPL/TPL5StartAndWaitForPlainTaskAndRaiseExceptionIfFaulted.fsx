open System
open System.Threading
open System.Threading.Tasks


module MyAsync = 
     let sleep (time) =
        // 'FromContinuations' is the basic primitive for creating workflows
        Async.FromContinuations(fun (cont, econt, ccont) ->
            // This code is called when workflow (this operation) is executed
            let tmr = new System.Timers.Timer(time, AutoReset=false)
            tmr.Elapsed.Add(fun _ -> 
                // Run the rest of the computation
                cont())
            tmr.Start() )

module Async =
    let awaitTaskUnit (t:Task) =
        (*
            This helper funcion allows integration of Task with F# asynchronous workflow. It wires up task's IsFaulted, IsCanceled and IsCompleted continuations with F# async's current exception, cancellation and success continuations. 
            NOTE: the IsCompleted check must be made after the IsFaulted check because a fault is also considered completed. – eulerfx Jan 29 '15 at 20:29
        *)

        Async.FromContinuations <| fun (ok,err,cnc) ->
            t.ContinueWith(fun t ->
            if t.IsFaulted then 
                printfn "faulted" 
                err(t.Exception)
            elif t.IsCanceled then cnc(OperationCanceledException("Task wrapped with Async.AwaitTask has been cancelled.",  t.Exception))
            elif t.IsCompleted then 
                printfn "completed"
                ok()
            else failwith "invalid Task state!"
            )
            |> ignore
   
    let inline awaitPlainTask (task:Task) =
        // Rethrow exception from preceding task if faulted
        let continuation (t: Task) : unit =
            match t.IsFaulted with
            | true ->
                printfn "t.IsFaulted=true" 
                raise t.Exception
            | arg -> 
                printfn "t.IsFaulted=false"
                ()

        task.ContinueWith continuation |> Async.AwaitTask
            
    let inline startAsPlainTask (work: Async<unit>) =
        Task.Factory.StartNew(fun () -> work |> Async.RunSynchronously)


let sleepy = async {
    do! Async.Sleep 2000
    printfn "awake"
    failwith "bomb"
}

let sleep2 = async {
  do! sleepy |> Async.startAsPlainTask |> Async.awaitPlainTask
}

let sleep3 = async {
    do!
    async {
        do! Async.Sleep 2000
        printfn "awake"
    } |> Async.startAsPlainTask |> Async.awaitTaskUnit
}

// call the workflows
sleepy |> Async.RunSynchronously
sleepy |> Async.startAsPlainTask |> ignore
sleep2 |> Async.Start
sleep3 |> Async.Start

//-------------------------------------------------//
// Async.Await not catching Task exception
// http://stackoverflow.com/questions/25166363/async-await-not-catching-task-exception
Task.Factory.StartNew(Action(fun _ -> failwith "oops"))
|> Async.AwaitIAsyncResult
|> Async.RunSynchronously

Task.Factory.StartNew(fun _ -> failwith "oops"                              
                               5)
|> Async.AwaitTask
|> Async.Ignore
|> Async.RunSynchronously


//------------------------------------------------//
// The following will not catch exception
let throwExceptionAsync() = async {
    raise <| new InvalidOperationException()  
}

let callThrowExceptionAsync() =
    try
        throwExceptionAsync() |> Async.Start
    with e ->
        printfn "Failed"

callThrowExceptionAsync ()