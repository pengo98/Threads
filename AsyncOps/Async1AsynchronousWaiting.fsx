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


async{
    printfn "Starting..."
    do! MyAsync.sleep 1000.0
    printfn "Finished!"  
}
|> Async.RunSynchronously