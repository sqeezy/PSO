open System

open Particle
open Swarm
open Models

[<EntryPoint>]
let main argv = 

    let func (parameters:ParameterSet):Fitnesse = parameters.[0] 
    let problem = {Func=func}

    let hub = Swarm problem
    let createParticle () = Particle hub problem
    let register w = hub.Post(Register w)
    let createAndRegister = createParticle >> register
    [1..20]
    |> fun _ -> createAndRegister()
    hub.Post(Start)
    while true do
        Console.WriteLine "wait"
        Threading.Thread.Sleep 1000
    0
