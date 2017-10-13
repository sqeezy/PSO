module Particle
open System

let ran = Random 42
let randomFloat ()=
  float(ran.Next(-10000000,10000000))
let Particle swarm problem = Agent.Start(fun inbox -> 

  let generateInitialSolution prob : Solution =
    //we need proper init here
    let parameters = [randomFloat()]
    let fitnesse = prob.Func parameters
    (parameters,fitnesse)

  let itterate (state:ParticleState) =
    let input = [randomFloat()]
    (input, state.Problem.Func input)

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
        let newTest = itterate state
        if isBetter newTest state.LocalBest then
          let newState = {state with LocalBest= newTest}
          if isBetter newTest state.GlobalBest then
            state.Swarm.Post(NewGlobalBest newTest)
          return! loop newState

    return! loop state
  }

  let initialState = (ParticleState.Create problem swarm (generateInitialSolution problem))
  loop initialState
)
