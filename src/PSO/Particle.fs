module Particle

let private randomBetweenZeroAndOne = random.NextDouble
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

//parameter order seems wrong - problem should be curried away
let itterate (problem:OptimizationProblem) (globalBest:Solution) (particle:Particle) =

  let weightLocal = randomBetweenZeroAndOne()
  let weightGlobal = randomBetweenZeroAndOne()
  let (globalBestPos, _) = globalBest
  let (localBestPos,localBestValue) = particle.LocalBest
  let pos = particle.Position

  let updatedVelocity = particle.Velocity 
                        ++ (weightGlobal .* (globalBestPos -- pos))
                        ++ (weightLocal  .* (localBestPos  -- pos))
                        |> limitVelocity problem.MaxVelocity

  let updatedPosition = pos 
                        ++ updatedVelocity
                        |> limitPosition problem.InputRange

  let updatedLocalBest =
    if problem.Func updatedPosition < localBestValue then
      (updatedPosition, problem.Func updatedPosition)
    else
      particle.LocalBest
  {
    Position  = updatedPosition;
    LocalBest = updatedLocalBest;
    Velocity  = updatedVelocity
  }
