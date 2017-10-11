[<AutoOpen>]
module Models

  type Agent<'T> = MailboxProcessor<'T>

  type ParticleMsg = 
    | Finish
    | UpdateGlobal of float
    | Start

  type SwarmMsg =
    |Register of Agent<ParticleMsg>
    |NewGlobalBest of float
    |Start

  type GlobalState = {Agents : Agent<ParticleMsg> list;GlobalBest : float} with
    member this.AddAgent agent = {Agents=agent::this.Agents;GlobalBest=this.GlobalBest}

  type ParticleState =
    {
      Swarm : Agent<SwarmMsg>;
      LocalBest : float;
      GlobalBest : float;
      Running : bool
    }
    with
    static member Create agent globalBest =
      {LocalBest=globalBest;GlobalBest=globalBest;Swarm=agent; Running = false}
