open System

module Math =
    let abs x =
        if x >= 0.0 then x
        else -x



(*
A common way to compute square roots is by Newton’s method of successive approximations. One starts with an initial guess y (say: y = 1). One then repeatedly improves the current guess y by taking the average of y and x/y.  
*)

let sqrt(x:float) =
    let goodEnough(guess: float) =
        Math.abs( guess * guess - x ) / x < 0.001

    let improve(guess: float): float =
        1./2. * (guess + x / guess)
        
    let rec sqrtIter(guess:float) =
        if goodEnough (guess:float) then guess
        else sqrtIter(improve guess)
     
    sqrtIter(1.0)

sqrt(2.0);;
sqrt(0.01);;
sqrt(1.0e-6);;
sqrt(1.0e60);;
