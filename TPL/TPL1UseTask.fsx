(*
https://sachabarbs.wordpress.com/2014/05/15/f-28-integrating-with-task-parallel-library/

How to Async module can be used with Task Parallel Library

1. TPL uses a Task<T> to represent a asynchronous operation that will return a value T

2. TPL uses a Task to represent a asynchronous operation that doesn’t return a value.

3. In TPL there are several trigger values that cause the Task<T> to be observed. Things like Wait / WaitAll / Result will also cause the tasks to be observed. These are however blocking operations that suspend the calling thread.

4. TPL may also use CancellationTokens to cancel async operations (albeit you need a bit more code in C# than you do in F# due to the fact that in C# you must constantly check the CancellationToken, which we saw in the previous post)

5. Both Task<T> and Task can be waited on

6. Both Task<T> and Task can run things known as continuations, which are essentially callbacks when the Task<T> / Task is done. You may schedule callback for when a Task ran to completion, or is faulted, or both, or none

7. Task<T> and Task for the basis of the new async/await syntax in C#

*)


(*
In the example below, we show how to create a simple Task<T> that returns a boolean. We will the use the blocking Task<T>.Wait() method, to obtain the result of the Task<T>, which will be a boolean in this case.
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

let result = task.Result
printfn "Task result is %O" result
