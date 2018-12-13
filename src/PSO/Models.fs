[<AutoOpen>]
module Models

open System

let random = Random(12345)
let randomBetweenZeroAndOne = random.NextDouble

type Fitnesse = float

type ParameterSet = float array

type TargetFunction = ParameterSet -> Fitnesse

type OptimizationProblem = {
  Description : string
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

type Log = string -> string -> unit

type Optimizer = OptimizationProblem -> Solution
