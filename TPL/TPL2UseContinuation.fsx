(*
We could also do this another way too which would yield the same results. We could use a continuation from the original Task<T> that is run when the original task runs to completion. Think of continuations as callbacks. Here is the code rewritten to use a continuation, remember you can have a single callback for the whole original task, or hook up specific ones for particular scenarios, which is what I have done here.
*)

open System
open System.Threading
open System.Threading.Tasks


let work () =
    for i in 0 .. 2 do
        printfn "Work loop is currently %O" i |> ignore
        Thread.Sleep 1000
    false

printfn "Starting task that returns a value" |> ignore

let task = Task.Factory.StartNew<bool>((fun () -> work () ), TaskCreationOptions.LongRunning)

let finalResult = task.ContinueWith((fun (antecedent:Task<bool>) -> printfn "Task result is %O" antecedent.Result ), TaskContinuationOptions.OnlyOnRanToCompletion)



