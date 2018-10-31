open PSO
open Models

let optimizer problem : Solution =

  let swarm = Swarm.create problem
  let iterParticleOnProblem = problem |> Particle.itterate
  let iterateSingle particle {GlobalBest = currentBest} = iterParticleOnProblem currentBest particle  
  let iterateAll (swarm:Swarm) : Swarm = swarm

  swarm.GlobalBest

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

    
    0
