open System

open Particle
open Swarm
open Models

[<EntryPoint>]
let Main argv = 

    let func (parameters:ParameterSet):Fitnesse = 
      let p1 = parameters.[0] 
      p1*p1
    let problem = {Func=func}

    let hub = Swarm problem
    let createParticle () = Particle hub problem
    let register w = hub.Post(Register w)
    let createAndRegister = createParticle >> register
    [1..100]
    |> fun _ -> createAndRegister()
    hub.Post(Start)
    while true do
        Console.WriteLine "wait"
        Threading.Thread.Sleep 1000
    0
