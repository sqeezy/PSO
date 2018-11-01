open PSO
open Models
open Particle
open Logging

let sequentialOptimize problem : Solution =

  let swarm = Swarm.create problem

  log "" (sprintf "Initial Swarm:")
  log "" (sprintf "%A" swarm)

  let iterParticleOnProblem = problem |> Particle.itterate
  let updateSingleParticle particle {GlobalBest = currentBest} = iterParticleOnProblem currentBest particle  
  let updateSingleAndApplyToSwarm swarm particle =
    let updatedSingleParticle = particle |> updateSingleParticle <| swarm
    let updatedSwarm = Swarm.update swarm updatedSingleParticle.LocalBest
    {updatedSwarm with Particles=(updatedSingleParticle::swarm.Particles |> Seq.toList)}
  let singleIterationOverWholeSwarm ({GlobalBest=globalBest; Particles=particles}) : Swarm = 
    let updatedSwarm = Seq.fold updateSingleAndApplyToSwarm {GlobalBest = globalBest;Particles=List.empty} particles
    log "" (sprintf "Mid Velocity %A" (List.averageBy (fun p -> p.Velocity |> Array.average) updatedSwarm.Particles))
    log "" ("CurrentBest "+ updatedSwarm.GlobalBest.ToString())
    updatedSwarm

  let dummyFolder swarm (i:int) = 
    log "" ("iteration "+ i.ToString())
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
