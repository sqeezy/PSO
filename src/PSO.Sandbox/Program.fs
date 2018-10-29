open System

open Particle
open PSO
open Models

[<EntryPoint>]
let Main argv = 

    let xSquared (parameters:ParameterSet):Fitnesse = 
      let p1 = Seq.item 0 parameters
      p1*p1
    let problem = {
      Func = xSquared;
      InputRange = (-5., 5.);
      Dimension = 1
    }
    let particles = [1..10] |> List.map (fun _ -> Particle.create problem)
    let swarm = Swarm.create particles

    let rec optimizer swarm problem =

      let iterParticleOnProblem = problem |> Particle.itterate

      let iterateSingle particle swarm =   particle |> iterParticleOnProblem swarm.GlobalBest 

      0
    
    0
