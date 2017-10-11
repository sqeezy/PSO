module Particle
open System

let ran = Random 42
let randomFloat ()=
  float(ran.Next(-10000000,10000000))
let Particle swarm = Agent.Start(fun inbox -> 
  let rec loop (state:ParticleState) = async{

    let! msg = inbox.TryReceive(1)

    match msg with
    |Some (value : ParticleMsg) ->
      match value with
        |ParticleMsg.Start -> return! loop {state with Running=true}
        |Finish -> return ()
        |UpdateGlobal newGlobal -> 
          let newState = {state with GlobalBest=newGlobal}
          return! loop newState
    |None -> 
      if state.Running then
        let newTest = Fitnesse (randomFloat())
        if newTest < state.LocalBest then
          let newState = {state with LocalBest= newTest}
          if newTest < state.GlobalBest then
            state.Swarm.Post(NewGlobalBest newTest)
          return! loop newState

    return! loop state
  }

  let initialState = (ParticleState.Create swarm (Fitnesse maxFloat))
  loop initialState
)
