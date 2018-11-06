module Particle

let private randomParam problem () = 
  let (min, max) = problem.InputRange
  min + (max - min)*randomBetweenZeroAndOne()
let private createParameters problem = 
  [1 .. problem.Dimension]
    |> Seq.map (fun _ -> randomParam(problem)())
    |> Seq.toArray
let create (problem : OptimizationProblem) =
  let startPosition = createParameters problem
  {Position = startPosition;LocalBest=(startPosition, problem.Func startPosition); Velocity=Array.zeroCreate problem.Dimension}

let (--) v1 v2 = Array.map2 (-) v1 v2
let (++) v1 v2 = Array.map2 (+) v1 v2
let (.*) scalar v = v |> Array.map (fun vCoordinate -> scalar * vCoordinate)

let private limitVelocity max (velocity : float array) =
  let norm v = v |> Array.sumBy (fun vc -> vc*vc) |> sqrt

  let veloLength = norm velocity

  match veloLength with
  | greater when greater > max -> velocity |>  ((.*) <| max/veloLength)
  | _                          -> velocity

let private limitPosition (min,max) (pos : float array) =
  let limitSingleCoordinate c = 
    match c with
    | underMin when underMin < min -> min
    | overMax  when overMax  > max -> max
    | _                            -> c
  pos |> Array.map limitSingleCoordinate


let itterate problem (globalBestPos,_) particle =

  let weightLocal = randomBetweenZeroAndOne()
  let weightGlobal = randomBetweenZeroAndOne()
  
  let (localBestPos, localBestValue) = particle.LocalBest
  let currentPosition = particle.Position

  let updatedVelocity = particle.Velocity 
                        ++ (weightGlobal .* (globalBestPos -- currentPosition ))
                        ++ (weightLocal  .* (localBestPos  -- currentPosition ))
                        |> limitVelocity problem.MaxVelocity

  let updatedPosition = currentPosition  
                        ++ updatedVelocity
                        |> limitPosition problem.InputRange

  let newCurrentValue = problem.Func updatedPosition
  let updatedLocalBest =
    if  newCurrentValue < localBestValue then
      (updatedPosition, newCurrentValue)
    else
      particle.LocalBest
      
  {
    Position  = updatedPosition;
    LocalBest = updatedLocalBest;
    Velocity  = updatedVelocity
  }
