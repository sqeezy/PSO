[<AutoOpen>]
module Models

  open System

  let maxFloat = Double.MaxValue

  type Agent<'T> = MailboxProcessor<'T>

  type Fitnesse = Fitnesse of float

  type ParameterSet = float list

  type TargetFunction = ParameterSet -> Fitnesse

  type Solution = ParameterSet * Fitnesse

  type ParticleMsg = 
    | Finish
    | UpdateGlobal of Fitnesse
    | Start

  type SwarmMsg =
    |Register of Agent<ParticleMsg>
    |NewGlobalBest of Fitnesse
    |Start

  type GlobalState = {Agents : Agent<ParticleMsg> list;GlobalBest : Fitnesse} with
    member this.AddAgent agent = {Agents=agent::this.Agents;GlobalBest=this.GlobalBest}

  type ParticleState =
    {
      Swarm : Agent<SwarmMsg>;
      LocalBest : Fitnesse;
      GlobalBest : Fitnesse;
      Running : bool
    }
    with
    static member Create agent globalBest =
      {LocalBest=globalBest;GlobalBest=globalBest;Swarm=agent; Running = false}
