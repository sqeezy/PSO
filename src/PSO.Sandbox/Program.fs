open System

open Worker
open Swarm
open Models

[<EntryPoint>]
let main argv = 
    let hub = Swarm()
    let createWorker () = Worker hub
    let register w = hub.Post(Register w)
    let createAndRegister = createWorker >> register
    [1..20]
    |> fun _ -> createAndRegister()
    hub.Post(Start)
    while true do
        Console.WriteLine "wait"
        Threading.Thread.Sleep 1000
    0
