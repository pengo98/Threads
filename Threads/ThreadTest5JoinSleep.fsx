open System
open System.Threading

(*
While waiting on a Sleep or Join, a thread is blocked and so does not consume CPU resources.
*)
(*
You can wait for another thread to end by calling its Join method. For example:
*)
let Go () =
    [1..1000] |> List.iter(fun _ -> printf "y")

let t = new Thread(Go)
t.Start()
t.Join()

printfn "Thread t has ended!"

(*
Thread.Sleep(0) relinquishes the thread’s current time slice immediately, voluntarily handing over the CPU to other threads. Framework 4.0’s new Thread.Yield() method does the same thing — except that it relinquishes only to threads running on the same processor.

Sleep(0) or Yield is occasionally useful in production code for advanced performance tweaks. It’s also an excellent diagnostic tool for helping to uncover thread safety issues: if inserting Thread.Yield() anywhere in your code makes or breaks the program, you almost certainly have a bug.
*)

