let numbers = seq {1..20}
let evensSqaured = Seq.choose(fun x -> 
                            match x with
                            | x when x%2=0 -> Some(x*x)
                            | _ -> None ) numbers

let evensSquared' = numbers
                    |> Seq.filter(fun x -> x%2=0)
                    |> Seq.map(fun x -> x * x)
printfn "numbers = %A\n" numbers
printfn "evensSquared = %A" evensSqaured
