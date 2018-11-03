open PSO
open Models
open Logging

let sequentialOptimize problem : Solution =

  let swarm = Swarm.create problem

  let iterParticleOnProblem = problem |> Particle.itterate
  let updateSingleParticle particle {GlobalBest = currentBest} = iterParticleOnProblem currentBest particle  
  let updateSingleAndApplyToSwarm swarm particle =
    let updatedSingleParticle = particle |> updateSingleParticle <| swarm
    let updatedSwarm = Swarm.update swarm updatedSingleParticle.LocalBest
    { updatedSwarm with Particles = (updatedSingleParticle::swarm.Particles) }
  let singleIterationOverWholeSwarm ({GlobalBest = globalBest; Particles = particles}) : Swarm = 
    Seq.fold updateSingleAndApplyToSwarm {GlobalBest = globalBest;Particles = List.empty} particles
  let dummyFolder swarm i = 
    log "" (sprintf "%A" swarm.Particles)
    singleIterationOverWholeSwarm swarm
  let swarmAfter10000 = Seq.fold dummyFolder swarm [1 .. 1000]
  swarmAfter10000.GlobalBest

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
      Dimension = 1
    }

    let solution = sequentialOptimize problem

    printfn "%A" solution
    
    0
