(*
Starting And Waiting For Plain Task

Another thing you might find yourself wanting to do is a use a TPL Task. That is a Task that does not return a value, basically you have Task<T> which is a task that returns T, and Task (essentially Task void, or Task<Unit> in F# lingo), which is a task that doesn’t return a value. Task may still be waited on in C# land, but there seems to be less you can do with a standard Task (one that doesn’t return a value) in F#.

There however a few tricks you can do, the first one requires a bit of insight into multi threading anyway, which is that Task, and Task<T> for that matter both implement IAsyncResult, which is something you can wait on inside of a F# async workflow, by using Async.AwaitIAsyncResult. Here is a small example, of how you can wait on a plain Task. This example also demonstrates how you can extend the Async module to include your own user specified functions. That is pretty cool actually, C# allows extension methods (which F# also allows), but being able to just add arbitrary functions is very cool.
*)


open System
open System.Threading
open System.Threading.Tasks

module Async =
//    let AwaitVoidTask (x:Task): Async<unit> =
//        x
//        |> Async.AwaitIAsyncResult
//        |> Async.Ignore       

    let AwaitVoidTask : (Task -> Async<unit>) =
        Async.AwaitIAsyncResult >> Async.Ignore
 

let theWorkflow (delay: int) = async {
    printfn "Starting workflow at %O" (DateTime.Now.ToLongTimeString())
    do! Task.Delay(delay) |> Async.AwaitVoidTask
    printfn "Ending workflow at %O" (DateTime.Now.ToLongTimeString())
}

Async.RunSynchronously (theWorkflow(2000))