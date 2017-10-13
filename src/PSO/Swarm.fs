module Swarm

let Swarm problem =
  Agent.Start(fun inbox -> 

    let sendNewBestToAllAgents (agents:list<Agent<ParticleMsg>>) best =
      let sendNewBestToAgent agent = (agent:Agent<ParticleMsg>).Post(UpdateGlobal best)
      agents |> List.map sendNewBestToAgent

    let rec loop state =  async{

      let! msg = inbox.TryReceive(1)

      match msg with
        |Some call -> 
          match call with
            |Register agent -> return! loop (state.AddAgent(agent):GlobalState)
            |Start -> 
              let startAgent (agent : Agent<ParticleMsg>) = agent.Post(ParticleMsg.Start)
              List.map startAgent state.Agents |> ignore
              return! loop state
            |NewGlobalBest value -> 
              match value < state.GlobalBest with
                |true ->
                  sendNewBestToAllAgents state.Agents value |> ignore
                  printfn "New global best %A" value
                  return! loop {state with GlobalBest=value}
                |false ->
                  return! loop state
        |None -> return! loop state
    }

    let initialSolution =
      let startParams = [0.0]
      (startParams, problem.Func startParams)

    loop {Agents=[];GlobalBest=initialSolution}
)
