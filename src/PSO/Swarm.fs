namespace PSO

open System

module Swarm =

  //from paper https://hal.archives-ouvertes.fr/file/index/docid/764996/filename/SPSO_descriptions.pdf
  let private typicalSwarmSize dimension = 10 + 2 * (dimension |> float |> Math.Sqrt |> int)
  let private valueFromSolution solution =
    let (_ , value) = solution
    value
  let private bestParticle particles = particles |> Seq.minBy (fun p -> p.LocalBest |> valueFromSolution)

  let create problem =
    let particles = [1 .. (typicalSwarmSize problem.Dimension)] 
                      |> List.map (fun _ -> Particle.create problem)
    let initialBestParticle = bestParticle particles
    {
      GlobalBest = initialBestParticle.LocalBest;
      Particles = particles
    }

  let update (swarm:Swarm) (proposal : Solution) : Swarm=
    let ( _, oldBest) = swarm.GlobalBest
    let ( _, proposedBest ) = proposal

    match proposedBest with
    | better when better < oldBest -> { swarm with GlobalBest = proposal }
    | worse  when worse >= oldBest -> swarm
    | _                            -> swarm
