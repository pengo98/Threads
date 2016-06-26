//    1
//   1 1
//  1 2 1
// 1 3 3 1
//1 4 6 4 1
//   ...
//
//
//The numbers at the edge of the triangle are all 1, and each number inside the triangle is the sum of the two numbers above it. Write a function that computes the elements of Pascal’s triangle by means of a recursive process.
//Do this exercise by implementing the pascal function in Main.scala, which takes a column c and a row r, counting from 0 and returns the number at that spot in the triangle. For example, pascal(0,2)=1,pascal(1,2)=2 and pascal(1,3)=3.

let rec pascal (c:int) (r: int) = 
    match c, r with
    | 0, _ -> 1
    | c, r ->
        if (c = r) then 1
        elif (c > r) then failwith "Index out of range exception"
        else
            pascal c (r-1) + (pascal (c-1) (r-1))


pascal 0 2;;
pascal 1 2;;
pascal 1 3;;
pascal 0 4;;
pascal 1 4;;
pascal 2 4;;
pascal 3 4;;
pascal 4 4;;
pascal 5 4;;
