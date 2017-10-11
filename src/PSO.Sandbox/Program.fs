open System

open Particle
open Swarm
open Models

[<EntryPoint>]
let main argv = 
    let hub = Swarm()
    let createParticle () = Particle hub
    let register w = hub.Post(Register w)
    let createAndRegister = createParticle >> register
    [1..20]
    |> fun _ -> createAndRegister()
    hub.Post(Start)
    while true do
        Console.WriteLine "wait"
        Threading.Thread.Sleep 1000
    0
