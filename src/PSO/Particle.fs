module Worker
open System

let maxFloat = Double.MaxValue
let ran = Random 42
let randomFloat ()=
  float(ran.Next(-10000000,10000000))

let Worker swarm = Agent.Start(fun inbox -> 
  let rec loop (state:WorkerState) = async{

    let! msg = inbox.TryReceive(1)

    match msg with
    |Some (value : WorkerMsg) ->
      match value with
        |WorkerMsg.Start -> return! loop {state with Running=true}
        |Finish -> return ()
        |UpdateGlobal newGlobal -> 
          let newState = {state with GlobalBest=newGlobal}
          return! loop newState
    |None -> 
      if state.Running then
        let newTest = randomFloat()
        if newTest < state.LocalBest then
          let newState = {state with LocalBest= newTest}
          if newTest < state.GlobalBest then
            state.Swarm.Post(NewGlobalBest newTest)
          return! loop newState

    return! loop state
    }

  let initialState = (WorkerState.Create swarm maxFloat) 
  loop initialState
)
