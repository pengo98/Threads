open System
open System.Threading

(*
When two threads simultaneously contend a lock (in this case, locker), one thread waits, or blocks, until the lock becomes available. In this case, it ensures only one thread can enter the critical section of code at a time, and “Done” will be printed just once. Code that's protected in such a manner — from indeterminacy in a multithreading context — is called thread-safe.
*)
(*
Shared data is the primary cause of complexity and obscure errors in multithreading. Although often essential, it pays to keep it as simple as possible.
*)

(*
open System.Threading
let lock (lockobj:obj) f =
  Monitor.Enter lockobj
  try
    f()
  finally
    Monitor.Exit lockobj  
*)

let monitor = new Object()
let mutable ``done`` = false

let Go () =
    if not ``done`` then
        ``done`` <- true
        printfn "Done"

let f () = lock monitor Go 

(new Thread(f)).Start()
f ()

