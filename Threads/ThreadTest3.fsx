open System
open System.Threading

(*
 This example illustrate another key concept: that of thread safety (or rather, lack of it!) The output is actually indeterminate:  “Done” could be printed twice. If you add Thread.Sleep between the new thread and main thread, then you only see "Done" printed once. 
*)

let mutable ``done`` = false

let Go () =
    if not ``done`` then
        ``done`` <- true
        printfn "Done"

(new Thread(Go)).Start()
//Thread.Sleep 1
Go ()

(* 
 The problem is that one thread can be evaluating the if statement right as the other thread is executing the WriteLine statement — before it’s had a chance to set done to true. 

*)