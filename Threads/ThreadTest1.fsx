#load "Library1.fs"
open Threads
open System
open System.Threading

let writeY () =
    [0..999] |> List.iter(fun _ -> printf "y")

let t = new Thread(writeY) //kick off a new thread
t.Start() //running writeY ()

// Simultaneous do something on main thread
[0..999] |> List.iter(fun _ -> printf "x")

