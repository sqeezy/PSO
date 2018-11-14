module SequentialOptimizer

open PSO
open Models
open System

  type Config = {
    MaxIterations : int
  }

  let create problem config (log: string -> string -> unit): Solution =
    let logLabeled tag msg = log (sprintf "%s | %s" problem.Description tag) msg 

    let startTime = DateTime.Now

    let iterParticleOnProblem = problem |> Particle.itterate
    
    let updateSingleParticle particle {GlobalBest = currentBest} = iterParticleOnProblem currentBest particle  
    
    let updateSingleAndApplyToSwarm swarm particle =
      let updatedSingleParticle = particle |> updateSingleParticle <| swarm
      let updatedSwarm = Swarm.update swarm updatedSingleParticle.LocalBest
      { updatedSwarm with Particles = (updatedSingleParticle::swarm.Particles) }
    
    let singleIterationOverWholeSwarm ({GlobalBest = globalBest; Particles = particles}) : Swarm = 
      Seq.fold updateSingleAndApplyToSwarm {GlobalBest = globalBest;Particles = List.empty} particles
      
    let trivialFold swarm i = 
      let updatedSwarm = singleIterationOverWholeSwarm swarm
      
      let avgVelocity = updatedSwarm.Particles |> List.averageBy (fun p -> (Array.averageBy abs p.Velocity))
      logLabeled (sprintf "average Velocity (%d)" i) (sprintf "%A" avgVelocity)
      
      updatedSwarm
      
    let swarm = Swarm.create problem
    
    let swarmAfterMaxIterations = Seq.fold trivialFold swarm [1 .. config.MaxIterations]
    
    let endTime = DateTime.Now
    let duration = (endTime - startTime).TotalSeconds


    logLabeled (sprintf "duration for %d iterations" config.MaxIterations) (sprintf "%A" duration)
    logLabeled "final Positions" (sprintf "%A" swarmAfterMaxIterations.Particles)
    
    swarmAfterMaxIterations.GlobalBest
