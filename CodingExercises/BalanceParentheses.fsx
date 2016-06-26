module String =
    /// Convert string to a list of characters
    let toCharList(s:string) =
        [for c in s -> c]

    let fromCharList(xs: List<char>) : string =
        let sb = new System.Text.StringBuilder()
        sb.Append(xs) |> ignore
        sb.ToString()



let balance(chars: List<char>): bool =
    let rec loop(chars: List<char>, score: int) = 
        match chars with
        | [] -> score// return result
        | '('::xs -> loop(xs, score + 1)
        | ')'::xs -> loop(xs, score - 1)
        | x::xs -> loop(xs, score)
    loop(chars, 0) = 0

let balance2(chars: List<char>): bool =
    let rec loop(chars: List<char>, score: int) = 
        //printfn "score: %i" score
        if score >= 0 then
            match chars with
            | [] -> score = 0 
            | '('::xs -> loop(xs, score + 1)
            | ')'::xs -> loop(xs, score - 1)
            | x::xs -> loop(xs, score)
        else
            false
    loop(chars, 0)

let chars1: List<char> = "(if (zero? x) max (/ 1 x))" |> String.toCharList
let chars2: List<char> = "I told him (that it’s not (yet) done). (But he wasn’t listening)" |>String.toCharList

let chars3: List<char> = ":-)" |> String.toCharList
let chars4: List<char> = "())(" |> String.toCharList

let result1 = balance(chars1)
let result2 = balance(chars2)
let result3 = balance(chars3)
let result4 = balance(chars4)

balance2(chars1)
balance2(chars2)
balance2(chars3)
balance2(chars4)
