module SequentialOptimizer

open PSO
open Models

  type Config = {
    MaxIterations : int
  }

  let create problem config (log: string -> string -> unit): Solution =

    let iterParticleOnProblem = problem |> Particle.itterate
    
    let updateSingleParticle particle {GlobalBest = currentBest} = iterParticleOnProblem currentBest particle  
    
    let updateSingleAndApplyToSwarm swarm particle =
      let updatedSingleParticle = particle |> updateSingleParticle <| swarm
      let updatedSwarm = Swarm.update swarm updatedSingleParticle.LocalBest
      { updatedSwarm with Particles = (updatedSingleParticle::swarm.Particles) }
    
    let singleIterationOverWholeSwarm ({GlobalBest = globalBest; Particles = particles}) : Swarm = 
      Seq.fold updateSingleAndApplyToSwarm {GlobalBest = globalBest;Particles = List.empty} particles
      
    let dummyFolder swarm i = 
      let updatedSwarm = singleIterationOverWholeSwarm swarm
      
      let avgVelocity = updatedSwarm.Particles |> List.averageBy (fun p -> Array.average p.Velocity)
      log "average Velocity: " (sprintf "%A" avgVelocity)
      
      updatedSwarm
      
    let swarm = Swarm.create problem
    
    let swarmAfter10000 = Seq.fold dummyFolder swarm [1 .. config.MaxIterations]
    
    log "final Positions" (sprintf "%A" swarmAfter10000.Particles)
    
    swarmAfter10000.GlobalBest
