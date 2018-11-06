open Models
open SequentialOptimizer
open Logging

let oneDimPolynoma =
  let xSquared (parameters:ParameterSet):Fitnesse = 
    let x = Seq.item 0 parameters
    // f(x) = x³ - 6x² + 4X + 12
    (x*x*x - 6.*x*x + 4.*x + 12.)
  {
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
    Func = xSquaredAndYSquared;
    InputRange = (-5., 5.);
    MaxVelocity=0.1;
    Dimension = 2
  }

let xSquaredProblem =
  let xSquared (parameters:ParameterSet):Fitnesse = 
    let p1 = Seq.item 0 parameters
    p1*p1
  {
    Func = xSquared;
    InputRange = (0., 5.);
    MaxVelocity=0.1;
    Dimension = 1
  }

let xSquaredYSquaredZSquaredProblem =
  let xxyyzz (parameters:ParameterSet):Fitnesse = 
    let p1 = Seq.item 0 parameters
    let p2 = Seq.item 1 parameters
    let p3 = Seq.item 2 parameters
    p1*p1+p2*p2+p3*p3
  {
    Func = xxyyzz;
    InputRange = (-5., 5.);
    MaxVelocity=0.1;
    Dimension = 3
  }

[<EntryPoint>]
let Main argv = 
    let config = {MaxIterations=1000}
    let solution = SequentialOptimizer.create xSquaredYSquaredZSquaredProblem config log
    printfn "%A" solution
    0
