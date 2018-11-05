open Models
open SequentialOptimizer
open Logging

let oneDimPolynoma =
  let xSquared (parameters:ParameterSet):Fitnesse = 
    let x = Seq.item 0 parameters
    (x*x*x - 6.*x*x + 4.*x + 12.)

  let sum (parameters:ParameterSet):Fitnesse = 
    Seq.sum parameters

  let problem = {
    Func = xSquared;
    InputRange = (-10., 10.);
    MaxVelocity=0.1;
    Dimension = 1
  }
  problem

let xSquaredYSquaredProblem =
  let xSquaredAndYSquared (parameters:ParameterSet):Fitnesse = 
    let p1 = Seq.item 0 parameters
    let p2 = Seq.item 0 parameters
    p1*p1+p2*p2

  let sum (parameters:ParameterSet):Fitnesse = 
    Seq.sum parameters

  let problem = {
    Func = xSquaredAndYSquared;
    InputRange = (-5., 5.);
    MaxVelocity=0.1;
    Dimension = 2
  }
  problem


let xSquaredProblem =
  let xSquared (parameters:ParameterSet):Fitnesse = 
    let p1 = Seq.item 0 parameters
    p1*p1

  let sum (parameters:ParameterSet):Fitnesse = 
    Seq.sum parameters

  let problem = {
    Func = xSquared;
    InputRange = (0., 5.);
    MaxVelocity=0.1;
    Dimension = 1
  }
  problem

[<EntryPoint>]
let Main argv = 
    
    let config = {MaxIterations=1000}
    
    let solution = SequentialOptimizer.create oneDimPolynoma config log

    printfn "%A" solution
    
    0
