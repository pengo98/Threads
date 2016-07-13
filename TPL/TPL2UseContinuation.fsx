(*
We could also do this another way too which would yield the same results. We could use a continuation from the original Task<T> that is run when the original task runs to completion. Think of continuations as callbacks. Here is the code rewritten to use a continuation, remember you can have a single callback for the whole original task, or hook up specific ones for particular scenarios, which is what I have done here.
*)

open System
open System.Threading
open System.Threading.Tasks


let work () =
    for i in 0 .. 2 do
        printfn "Work loop is currently %O" i |> ignore
        Thread.Sleep 1000
    false

printfn "Starting task that returns a value" |> ignore

let task = Task.Factory.StartNew<bool>((fun () -> work () ), TaskCreationOptions.LongRunning)

let finalResult = task.ContinueWith((fun (antecedent:Task<bool>) -> printfn "Task result is %O" antecedent.Result ), TaskContinuationOptions.OnlyOnRanToCompletion)




//----------------------------------------------------------------------


//--------------------------------------------------

//let countChange(money: int, coins: List<int>): int =
//    let rec loop(amount: int, coins: List<int>, n: int) =
//        if amount = 0 then
//            match coins with
//            | [] -> n
//            | x::xs -> loop(money, xs, n) 
//        else
//            match coins with
//            |[] -> n 
//            |x::xs ->
//                if (amount-x > 0) then
//                    loop(amount-x, [x], if (amount-x)=0 then n+1 else 0)
//                else  
//                    loop(amount-x, xs, if (amount-x)=0 then n+1 else 0)
//    loop(money, coins, 0)
//
//
//countChange(0, [1;2]);;
//countChange(1, [1]);;
//countChange(1, [1;2]);;
//countChange(2, [1;2]);;
//countChange(2, [1;2;5])
//countChange(4, [1;2])
//4-1=3
//3-1=2
//2-1=1
//1-1=0


let firstDenomination kindOfCoins =
    match kindOfCoins with
    | 1 -> 1
    | 2 -> 5
    | 3 -> 10
    | 4 -> 25
    | 5 -> 50
    | _ -> failwith "Argument out of range exception"


(*
This problem has a simple solution as a recursive procedure. Suppose we think of the types of coins
available as arranged in some order. Then the following relation holds:
The number of ways to give change of amount a using n kinds of coins equals
the number of ways to give change of amount a using all but the first kind of coin, plus
the number of ways to give change of amount a - d using all n kinds of coins, where d is the
denomination of the first kind of coin.
To see why this is true, observe that the ways to make change can be divided into two groups: those
that do not use any of the first kind of coin, and those that do. Therefore, the total number of ways to
make change for some amount is equal to the number of ways to make change for the amount without
using any of the first kind of coin, plus the number of ways to make change assuming that we do use
the first kind of coin. But the latter number is equal to the number of ways to make change for the
amount that remains after using a coin of the first kind.
Thus, we can recursively reduce the problem of changing a given amount to the problem of changing
smaller amounts using fewer kinds of coins. Consider this reduction rule carefully, and convince
yourself that we can use it to describe an algorithm if we specify the following degenerate cases: 
If a is exactly 0, we should count that as 1 way to make change.
If a is less than 0, we should count that as 0 ways to make change.
If n is 0, we should count that as 0 ways to make change.  
*)

let countChange (amount:int) (coins: List<int>) =

    let rec cc (amount:int) (coins: List<int>) = 
        if amount = 0 then 1
        elif amount < 0 || coins.Length = 0 then 0
        else cc amount (coins.Tail) + cc (amount - coins.Head) coins

    cc amount coins

countChange 10 [3] = 0;;
countChange 5 [5] = 1;;
countChange 10 [1;5] = 3;;
countChange 4 [1;2] = 3;;


countChange 300 [5;10;20;50;100;200;500] = 1022;;
countChange 301 [5;10;20;50;100;200;500] = 0;;
countChange 300 [500;5;50;100;20;200;10] = 1022;;
//-------------------------------

