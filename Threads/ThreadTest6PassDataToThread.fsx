open System
open System.Threading

let print message =
    printfn "%s" message


let t = new Thread(fun () -> print "Hello from t")
t.Start()