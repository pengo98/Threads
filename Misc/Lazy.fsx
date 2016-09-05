(*
The let construct in Haskell looks superficially like the one in F#, but semantically it is very different because it's non-strict ("lazy") by default. This means let x = 4 in 2 * x in Haskell is more appropriately translated into let x = lazy(4) in 2 * Lazy.force x.
*) 
let x = 4 in 2 * x
let x' = lazy(4) in 2 * x'.Force()

// let binding and continuation 
let z =
    let x = lazy(2) in
        let y = 2 * x.Force() in
            x.Force() + y


// Be careful when mixing side effect and laziness as lazy caches the result and will only execute side effect once.             
let lazySideEffect =
    lazy (
        let temp = 2 + 2
        printfn "%i" temp
        temp
    )

printfn "Force value the first time"
lazySideEffect.Force()
printfn "Force vlaue the second time"
lazySideEffect.Force()
