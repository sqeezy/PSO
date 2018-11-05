module Particle

let randomBetweenZeroAndOne = random.NextDouble

let create (problem : OptimizationProblem) =

  let randomParam () = 
    let (min, max) = problem.InputRange
    min + (max - min)*randomBetweenZeroAndOne()
  
  let parameters = 
    [1 .. problem.Dimension]
      |> Seq.map (fun _ -> randomParam())
      |> Seq.toArray

  {Position = parameters;LocalBest=(parameters, problem.Func parameters); Velocity=Array.zeroCreate problem.Dimension}

let (--) seq1 seq2 = Array.map2 (-) seq1 seq2
let (++) seq1 seq2 = Array.map2 (+) seq1 seq2
let (.*) scalar s =
  let mulByScalar a = a * scalar
  s |> Array.map mulByScalar

let limitVelocity max (velocity : float array) =
  let norm v = v |> Array.sumBy (fun vc -> vc*vc) |> sqrt

  let veloLength = norm velocity

  match veloLength with
  | greater when greater > max -> velocity |>  ((.*) <| max/veloLength)
  | _                          -> velocity

let limitPosition (min,max) (pos : float array) =
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
