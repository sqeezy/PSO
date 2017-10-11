[<AutoOpen>]
module Models

  type Agent<'T> = MailboxProcessor<'T>

  type WorkerMsg = 
    | Finish
    | UpdateGlobal of float
    | Start

  type SwarmMsg =
    |Register of Agent<WorkerMsg>
    |NewGlobalBest of float
    |Start

  type GlobalState = {Agents : Agent<WorkerMsg> list;GlobalBest : float} with
    member this.AddAgent agent = {Agents=agent::this.Agents;GlobalBest=this.GlobalBest}

  type WorkerState =
    {
      Swarm : Agent<SwarmMsg>;
      LocalBest : float;
      GlobalBest : float;
      Running : bool
    }
    with
    static member Create agent globalBest =
      {LocalBest=globalBest;GlobalBest=globalBest;Swarm=agent; Running = false}
