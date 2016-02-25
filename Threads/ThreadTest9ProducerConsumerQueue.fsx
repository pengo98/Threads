open System
open System.Threading
open System.Collections.Generic

type ProducerConsumerQueue() =
    let wh = new AutoResetEvent(false)
    let sync = ref 0
    let tasks = new Queue<string option>()
    let mutable worker = Unchecked.defaultof<_>
    let dequeue() =
            lock sync (fun _ -> 
                if (tasks.Count > 0) then
                    tasks.Dequeue() 
                else
                    None
            )

    let work() =
        let mutable continueLooping = true
        while continueLooping do
            let task = dequeue()
            match task with
            | Some t -> 
                    printfn "Performing task %s: " t
                    Thread.Sleep 1000
            | None ->
                    printfn "stop looping"
                    continueLooping <- false 
                    wh.WaitOne() |> ignore


    do
        worker <- new Thread(work)
        worker.Start()

    member x.EnqueueTask(task) =
        lock sync (fun _ -> tasks.Enqueue task)
        wh.Set() |> ignore

    interface IDisposable with
        member x.Dispose() =
            printfn "Calling dispose"
            x.EnqueueTask None |> ignore
            worker.Join()
            wh.Close()

let main() =
    (
        use q = new ProducerConsumerQueue()
        q.EnqueueTask (Some "Hello")
        [1..10] |> List.iter(fun x ->
                                let y = Some (sprintf "Say %i" x) 
                                q.EnqueueTask y )
        q.EnqueueTask (Some "Goodbye") |> ignore
//        Thread.Sleep 10000  
    )
    

main()