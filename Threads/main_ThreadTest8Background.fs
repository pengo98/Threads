module BackGroundThread
    open System
    open System.Threading

    (*
    By default, threads you create explicitly are foreground threads. Foreground threads keep the application alive for as long as any one of them is running, whereas background threads do not. Once all foreground threads finish, the application ends, and any background threads still running abruptly terminate.
    *)

    (*
    If this program is called with no arguments, the worker thread assumes foreground status and will wait on the ReadLine statement for the user to press Enter. Meanwhile, the main thread exits, but the application keeps running because a foreground thread is still alive.

    On the other hand, if an argument is passed to Main(), the worker is assigned background status, and the program exits almost immediately as the main thread ends (terminating the ReadLine).

    When a process terminates in this manner, any finally blocks in the execution stack of background threads are circumvented. This is a problem if your program employs finally (or using) blocks to perform cleanup work such as releasing resources or deleting temporary files. To avoid this, you can explicitly wait out such background threads upon exiting an application. There are two ways to accomplish this:

    If you’ve created the thread yourself, call Join on the thread.
    If you’re on a pooled thread, use an event wait handle.

    In either case, you should specify a timeout, so you can abandon a renegade thread should it refuse to finish for some reason. This is your backup exit strategy: in the end, you want your application to close — without the user having to enlist help from the Task Manager!

    Foreground threads don’t require this treatment, but you must take care to avoid bugs that could cause the thread not to end. A common cause for applications failing to exit properly is the presence of active foreground threads.
    *)


    [<EntryPoint>]
    let main (args:string []) =

        let worker = new Thread(fun () -> Console.ReadLine() |> ignore)
        printfn "%A" args
        Thread.CurrentThread.Name <- "main"
        worker.Name <- "worker"
        
        if args.Length > 0 then
            printfn "set worker to background thread"
            worker.IsBackground <- true
        else
            printfn "set worke to foregorund thread. Should not exit until user enters newline"
            worker.IsBackground <- false

        printfn "worker.IsBackground: %A" worker.IsBackground
        printfn "main.IsBackground: %A" Thread.CurrentThread.IsBackground
        worker.Start()
//        worker.Join()

        0
