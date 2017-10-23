namespace PSO

open System
open Particle

module Swarm =

  //from paper https://hal.archives-ouvertes.fr/file/index/docid/764996/filename/SPSO_descriptions.pdf
  let swarmSize dimension = 10 + 2 * (Math.Sqrt dimension |> int)

  let Swarm particles =
    let initialBestParticle = particles |> Seq.minBy (fun p -> p.LocalBest |> valueFromSolution)
    {GlobalBest=initialBestParticle.LocalBest;Particles=particles}

  let update swarm proposal : (bool*Swarm)=
    let oldBest = swarm.GlobalBest
    if valueFromSolution proposal < valueFromSolution oldBest then
      (true,{swarm with GlobalBest=proposal})
    else
      (false,swarm)
