

module Prelude =
        /// Null-coalescing operator.
    let (|?) lhs rhs = if lhs = null then rhs else lhs

    /// Implicit operator.
    let inline (!>) (x:^a) : ^b = ((^a or ^b) : (static member op_Implicit : ^a -> ^b) x)

    /// Explicit operator.
    let inline (!!) (a:^a) : ^b = ((^a or ^b) : (static member op_Explicit : ^a -> ^b) a)


//let inline (!>) (x:^a) : ^b = ((^a or ^b) : (static member op_Implicit : ^a -> ^b) x)
open Prelude
type A() = class end
type B() = static member op_Implicit(a:A) = B()

let myfn (b : B) = "result"

(* apply the implicit conversion to an A using our operator, then call the function *)
myfn (!> A())

myfn( B.op_Implicit(A()) ) 


type Async<'a> = ('a -> unit) -> unit

let create (a: 'a): Async<'a> =
    fun (cb: 'a -> unit) ->
        cb a

let map (f: 'a -> 'b) (a: Async<'a>) : Async<'b> =
    fun (cb: 'b -> unit) ->
        a (fun (a: 'a) -> cb (f a) )
//         (fun (a: 'a) -> a |> f |> cb ) |> a

let 

let bind (f: 'a -> Async<'b>) (a:Async<'a>) : Async<'b> =
    fun (cb: 'b -> unit) ->
        a (fun (a:'a) ->
           let b:Async<'b> = f a
           b (fun (b:'b) -> cb b) )


//type Async<'a> = ('a -> unit) -> unit
module Async =
    open System.Threading
    open System.Threading.Tasks


//    let create (a:'a) : Async<'a> =
//        fun (cb: 'a -> unit) -> cb a
            
    // Async is like callback.
    let create (a:'a) : Async<'a> =
        fun (cb:'a -> unit) ->
            cb a

            

    let map (f: 'a -> 'b) (a:Async<'a>) : Async<'b> =
        fun (cb: 'b -> unit) ->
            a (fun (a: 'a) -> cb (f a))

//    let map (f: 'a -> 'b) (a:Async<'a>) : Async<'b> =
//        fun (cb: 'b -> unit) ->
//            a (fun (a: 'a) -> cb (f a))
//
//    let map' (f: 'a -> 'b) (a:Async<'a>) : Async<'b> =
//        fun (cb: 'b -> unit) ->
//            let d = fun (a: 'a) -> cb (f a)
//            let e = a (d)
//            a (fun (a: 'a) -> cb (f a))
//            d

open System
open System.Net
open System.Threading
open System.Threading.Tasks

let address = new Uri("http://www.google.com")
let client = new WebClient()
client.AsyncDownloadString(address)

let downloadStringAsync (address:string) : Async<string>= 
    // fetch the string from somewhere
    Thread.Sleep 500
    
    Async.create "result "
