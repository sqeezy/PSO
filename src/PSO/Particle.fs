module Particle
open System

let randomBetweenZeroAndOne = random.NextDouble

let create (problem : OptimizationProblem) =

  let randomParam () = 
    let (min, max) = problem.InputRange
    min + (max - min)*randomBetweenZeroAndOne()
  
  let parameters = 
    [1..problem.Dimension]
      |> Seq.map (fun _ -> randomParam())

  {Position = parameters;LocalBest=(parameters, problem.Func parameters); Velocity=Array.zeroCreate problem.Dimension}

let (--) seq1 seq2 = Seq.map2 (-) seq1 seq2
let (++) seq1 seq2 = Seq.map2 (+) seq1 seq2
let (.*) scalar s =
  let mulByScalar a = a * scalar
  s |> Seq.map mulByScalar

//parameter order seems wrong - problem should be curried away
let itterate (problem:OptimizationProblem)(globalBest:Solution) (particle:Particle) =

  let weightLocal = randomBetweenZeroAndOne()
  let weightGlobal = randomBetweenZeroAndOne()
  let (globalBestPos, _) = globalBest
  let (localBestPos,localBestValue) = particle.LocalBest
  let pos = particle.Position

  let updatedVelocity = particle.Velocity 
                        ++ (weightGlobal .* (globalBestPos -- pos))
                        ++ (weightLocal  .* (localBestPos  -- pos)) 

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
