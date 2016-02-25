open System
open System.Threading
open System.Collections.Generic

type ProducerConsumerQueue() =
    let queue = new Queue<string option>()
    let mutable worker = Unchecked.defaultof<_>
    let sync = ref 0
    let wh = new AutoResetEvent(false)

    let dequeue() =
        lock sync (fun _ -> 
            if (queue.Count > 0) then
                queue.Dequeue()
            else
                None  
        )
        
    let work() =
        let mutable continueLoop = true
        while continueLoop do
            let task = dequeue()
            match task with
            | Some "end" ->
                    printfn "Worker finished for the day"
                    continueLoop <- false
            | Some t -> 
                    printfn "Processing task %s" t
                    Thread.Sleep 1000
            | None ->
                    printfn "No item in queue. Worker blocked"
                    wh.WaitOne() |> ignore

    do
        worker <- new Thread(work)
        worker.Start()


    member x.Enqueue(task) =
        lock sync (fun _ -> queue.Enqueue task
                            wh.Set() |> ignore
        )
        

    interface IDisposable with
        member x.Dispose() =
            worker.Join()
            wh.Close() 


let main() =
    (
        use q = new ProducerConsumerQueue()
        q.Enqueue (Some "Hello")
        [1..10] |> List.iter(fun x ->
                                let y = Some (sprintf "Say %i" x) 
                                q.Enqueue y )
        q.Enqueue (Some "Goodbye")
        q.Enqueue (Some "end")
//        Thread.Sleep 10000  
    )
    
main()

// Other ways to call ProducerConsumerQueue()
(********************************************
let q = new ProducerConsumerQueue()
q.Enqueue(Some "hello")
q.Enqueue(Some "hello2")
[1..3] |> List.iter(
            fun x ->
                q.Enqueue (Some (sprintf "item %i" x) ))
q.Enqueue(Some "end")
*********************************************)

(********************************************
using(new ProducerConsumerQueue()) ( fun q ->
  q.Enqueue(Some "hello")
  [1..10] |> List.iter(fun x ->
                            q.Enqueue (Some (sprintf "item %i" x))
                            )
  q.Enqueue(Some "good bye") 
  q.Enqueue(Some "end")
)

*********************************************)