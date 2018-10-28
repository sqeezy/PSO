[<AutoOpen>]
module Models

  open System

  let maxFloat = Double.MaxValue

  let random = Random()

  type Fitnesse = float

  type ParameterSet = float seq

  type TargetFunction = ParameterSet -> Fitnesse

  type OptimizationProblem = {
    Func : TargetFunction
    InputRange : float * float
    Dimension : int
  }

  type Solution = ParameterSet * Fitnesse

  type Particle = {
    Position : ParameterSet
    LocalBest : Solution
    Velocity : float seq
  }
  
  type Swarm = {
    GlobalBest : Solution;
    Particles : Particle seq
  }

  type Optimizer = OptimizationProblem -> Solution
