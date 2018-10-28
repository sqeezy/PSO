namespace PSO

open System

module Swarm =

  //from paper https://hal.archives-ouvertes.fr/file/index/docid/764996/filename/SPSO_descriptions.pdf
  let swarmSize dimension = 10 + 2 * (Math.Sqrt dimension |> int)

  let Swarm particles =
    let valueFromSolution solution =
      let (_ , value) = solution
      value
    let initialBestParticle = particles |> Seq.minBy (fun p -> p.LocalBest |> valueFromSolution)
    
    {
      GlobalBest = initialBestParticle.LocalBest;
      Particles = particles
    }

  let update (swarm:Swarm) (proposal : Solution) : (bool * Swarm)=
    let ( _, oldBest) = swarm.GlobalBest
    let ( _, proposedBest ) = proposal

    match proposedBest with
    | better when better < oldBest -> (true, { swarm with GlobalBest = proposal })
    | worse  when worse >= oldBest -> (false,swarm)
    | _                            -> (false, swarm)
