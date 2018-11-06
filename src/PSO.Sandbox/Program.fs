open Models
open SequentialOptimizer
open Logging

let oneDimPolynoma =
  let xSquared (parameters:ParameterSet):Fitnesse = 
    let x = Seq.item 0 parameters
    (x*x*x - 6.*x*x + 4.*x + 12.)
  {
    Description = "x³-6x²+4x+12"
    Func = xSquared;
    InputRange = (-1., 10.);
    MaxVelocity=0.1;
    Dimension = 1
  }

let xSquaredYSquaredProblem =
  let xSquaredAndYSquared (parameters:ParameterSet):Fitnesse = 
    let p1 = Seq.item 0 parameters
    let p2 = Seq.item 1 parameters
    p1*p1+p2*p2
  {
    Description = "x²+y²"
    Func = xSquaredAndYSquared
    InputRange = (-5., 5.)
    MaxVelocity=0.1
    Dimension = 2
  }

let xSquaredProblem =
  let xSquared (parameters:ParameterSet):Fitnesse = 
    let p1 = Seq.item 0 parameters
    p1*p1
  {
    Description = "x²"
    Func = xSquared
    InputRange = (0., 5.)
    MaxVelocity=0.1
    Dimension = 1
  }

let xSquaredYSquaredZSquaredProblem =
  let xxyyzz (parameters:ParameterSet):Fitnesse = 
    let p1 = Seq.item 0 parameters
    let p2 = Seq.item 1 parameters
    let p3 = Seq.item 2 parameters
    p1*p1+p2*p2+p3*p3
  {
    Description = "x²+y²+z²"
    Func = xxyyzz
    InputRange = (-5., 5.)
    MaxVelocity=0.1
    Dimension = 3
  }

let testProblem problem =
    let config = {MaxIterations=1000}
    let solution = SequentialOptimizer.create problem config log
    solution |> ignore

[<EntryPoint>]
let Main argv = 
    testProblem oneDimPolynoma
    testProblem xSquaredProblem
    testProblem xSquaredYSquaredProblem
    testProblem xSquaredYSquaredZSquaredProblem
    0
