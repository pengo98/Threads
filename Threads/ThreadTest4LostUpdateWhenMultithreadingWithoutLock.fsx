(*
 Lost update is possible if multiple threads update a shared variable without a lock. 
*)

(*
Part1: Multithreading without locking (result is indeterministic) 
*)
open System.Threading
let mutable x = 0
let increment() = x <- x + 1
let decrement() = x <- x - 1


let increment1000 = 
    fun () ->  
    [1..10000] |> List.iter(fun _ -> increment())
    printfn "Increment Thread %i completed" Thread.CurrentThread.ManagedThreadId

let decrement1000 =
    fun () ->
    [1..10000] |> List.iter(fun _ -> decrement())
    printfn "Decrement Thread %i completed" Thread.CurrentThread.ManagedThreadId

printfn "Inital x = %i" x
let t = new Thread(increment1000)
t.Start()

decrement1000() // runs on main thread

t.Join()
printfn "Final x = %i" x



(*
Part2: Proper locking  
*)
//open System.Threading
//let mutable y = 0
//let sync = ref 0
//let increment () = lock(sync) (fun _ -> y <- y + 1)
//let decrement () = lock(sync) (fun _ -> y <- y - 1)
//
//printfn "x = %i" y
//
//ThreadPool.QueueUserWorkItem(fun _ -> 
//                                [1..1000] 
//                                |> List.iter(fun _ -> increment() )
//                                printfn "Thread %i done" Thread.CurrentThread.ManagedThreadId   
//                             ) |> ignore
//
//ThreadPool.QueueUserWorkItem(fun _ -> 
//                                [1..1000] 
//                                |> List.iter(fun _ -> decrement() )
//                                printfn "Thread %i done" Thread.CurrentThread.ManagedThreadId   
//                             ) |> ignore
//
//printfn "x = %i" y
