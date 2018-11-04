open Models
open SequentialOptimizer
open Logging


[<EntryPoint>]
let Main argv = 

    let xSquared (parameters:ParameterSet):Fitnesse = 
      let p1 = Seq.item 0 parameters
      p1*p1

    let sum (parameters:ParameterSet):Fitnesse = 
      Seq.sum parameters

    let problem = {
      Func = xSquared;
      InputRange = (-5., 5.);
      MaxVelocity=0.1;
      Dimension = 1
    }
    
    let config = {MaxIterations=1000}
    
    let solution = SequentialOptimizer.create problem config log

    printfn "%A" solution
    
    0
