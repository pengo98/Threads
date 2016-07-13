
type Async<'a> = ('a -> unit) -> unit

module Async =

    open System.Threading
    open System.Threading.Tasks

    let spawn (action:unit->unit) : unit =
        ThreadPool.QueueUserWorkItem (fun _ -> action ()) |> ignore

    // Async is like callback.
    let create (a:'a) : Async<'a> =
        fun (cb:'a -> unit) ->
            cb a

    let map (f: 'a -> 'b) (a:Async<'a>) : Async<'b> =
        fun (cb: 'b -> unit) ->
            a (fun (a: 'a) -> cb (f a))

    let bind (f: 'a -> Async<'b>) (a:Async<'a>): Async<'b> =
        fun (cb: 'b -> unit) ->
            a (fun (a: 'a) ->
              let b: Async<'b> = f a
              b (fun (b: 'b) -> cb b)            )


    let bind' (f: 'a -> Async<'b>) (a:Async<'a>): Async<'b> =
        fun (cb: 'b -> unit) ->
            a (fun (a: 'a) ->
              let b: Async<'b> = f a
              spawn (fun () -> (fun (b: 'b) -> cb b) )           )

    // like async.runcontinuations
    let run (a: Async<'a>) : 'a =
        let tcs = new TaskCompletionSource<'a> ()
        a (fun (a:'a) -> tcs.SetResult a)
        tcs.Task.Result

    // like Async.runsynchronously
    let run' (a: Async<'a>) : 'a =
        let mre = new ManualResetEvent(false)
        let result = ref None
        a (fun (a:'a) ->
          result := Some a
          mre.Set() |> ignore)
        mre.WaitOne() |> ignore
        result.Value.Value


    // like Async.runsynchronously
    // but runs on a different thread using spawn
    // defeats the purpose of Async as it blocks a thread.
    // only ok to use it in the outer edge of your program.
    let run'' (a: Async<'a>) : 'a =
        let mre = new ManualResetEvent(false)
        let result = ref None
        spawn (fun () -> a (fun (a:'a) ->
              result := Some a
              mre.Set() |> ignore))
        mre.WaitOne() |> ignore
        result.Value.Value

    //let AwaitEvent (e:IEvent<'a>) : Async<'a> =

    // how to run independent computations in parallel
    // by evaluating the outer async, you kick it off.
    let StartChild (a:Async<'a>) : Async<Async<'a>> =
        failwith ""

    // distribute law of algerbra : a (b + c) => ab + ac
    // opposite of distribute law of algebra
    // like zip
    let Parallel (a:Async<'a>, b:Async<'b>) : Async<'a * 'b> =
        async {

            // fork
            let! a = Async.StartChild a //parallel
            let! b = Async.StartChild b //parallel
            // join
            let! a = a 
            let! b = b
            return a,b   
        }

    let Parallel' (a:Async<'a>, b:Async<'b>, c:Async<'c>) : Async<'a * 'b * 'c> =
        async {

            // fork
            let! ab = Async.StartChild (Parallel (a, b))
            let! c = Async.StartChild c //parallel
            // join
            let! a,b = ab
            let! c = c
            return a,b,c   
        }

    let ParallelAll (xs:Async<'a>[]) : Async<'a[]> =
       Async.Parallel xs 

    let ParallelAll' (xs:Async<'a>[]) : Async<'a[]> =
       let t: Task<'a> = failwith ""
       Async.Parallel xs 


    let doStuff =
        // printfn will be evaluated right away as it's not part of async and will be eagerly evaluated. 
        printfn "hello"
        async {
          let x = 1 + 2
          return x
        }
    doStuff |> Async.RunSynchronously

    let doStuff' =
        // everything insdie async is lazily evaluated. using Async.Delay()
        async {
          printfn "hello"
          let x = 1 + 2
          return x
        }
    doStuff' |> Async.RunSynchronously

