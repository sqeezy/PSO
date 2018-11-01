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
let clamp min max velocity =
  match velocity with
  | v when v < min -> min
  | v when v > max -> max
  | _              -> velocity


//parameter order seems wrong - problem should be curried away
let itterate (problem:OptimizationProblem) (globalBest:Solution) (particle:Particle) =

  let weightLocal = randomBetweenZeroAndOne()
  let weightGlobal = randomBetweenZeroAndOne()
  let (globalBestPos, _) = globalBest
  let (localBestPos,localBestValue) = particle.LocalBest
  let pos = particle.Position
  let (min, max) = problem.InputRange
  let range = max - min
  let maxAbsoluteVelo = range/10.

  let updatedVelocity = particle.Velocity 
                        ++ (weightGlobal .* (globalBestPos -- pos))
                        ++ (weightLocal  .* (localBestPos  -- pos)) 
                        |> Array.map (clamp -maxAbsoluteVelo maxAbsoluteVelo)

  let updatedPosition = pos ++ updatedVelocity

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
