[<AutoOpen>]
module Models

open System

let random = Random()

type Fitnesse = float

type ParameterSet = float array

type TargetFunction = ParameterSet -> Fitnesse

type OptimizationProblem = {
  Func : TargetFunction
  InputRange : float * float
  MaxVelocity: float
  Dimension : int
}

type Solution = ParameterSet * Fitnesse

type Particle = {
  Position : ParameterSet
  LocalBest : Solution
  Velocity : float array
}

type Swarm = {
  GlobalBest : Solution;
  Particles : Particle list
}

type Optimizer = OptimizationProblem -> Solution
