// Deadlock example
(*
Thread 1 has acquired lock1 to access a critical region. In the same region, Thread 1 tries to call a method which reqires lock2 to access.  

Thread 2 has acquired lock2 to access a critical region. In the same region, Thread 2 tries to call a method/function which requires lock1 to access.

Here is an alternative way to perceive the problem: when you call out to other code while holding a lock, the encapsulation of that lock subtly leaks. This is not a fault in the CLR or .NET Framework, but a fundamental limitation of locking in general. The problems of locking are being addressed in various research projects, including Software Transactional Memory
*)
open System.Threading
let sync1 = ref 0
let sync2 = ref 0

let t1 = new Thread(
            fun () ->
                printfn "t1 thread waiting to acquire lock1" 
                lock(sync1) (
                    fun _ -> 
                        printfn "t1 thread acquired lock1"
                        Thread.Sleep 1000
                        // the following code could be called in another function or method.
                        lock(sync2) (fun _ -> printfn "t1 thread acquired lock2" ) ))

t1.Start()

printfn "Main thread waiting to acquire lock2"
lock(sync2) (
    fun _ -> 
        printfn "Main thread acquired lock2"
        Thread.Sleep 1000
        // the following code could be called in another function or method.
        lock(sync1) (fun _ -> printfn "Main thread waiting to acquire lock1")   
)

