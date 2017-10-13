[<AutoOpen>]
module Models

  open System

  let maxFloat = Double.MaxValue

  type Agent<'T> = MailboxProcessor<'T>

  type Fitnesse = float

  type ParameterSet = float list

  type TargetFunction = ParameterSet -> Fitnesse

  type OptimizationProblem = {Func : TargetFunction}

  type Solution = ParameterSet * Fitnesse

  type ParticleMsg = 
    | Finish
    | UpdateGlobal of Solution
    | Start

  type SwarmMsg =
    |Register of Agent<ParticleMsg>
    |NewGlobalBest of Solution
    |Start

  type GlobalState = 
    {
      Agents : Agent<ParticleMsg> list;
      GlobalBest : Solution
    }
    with
    member this.AddAgent agent = {Agents=agent::this.Agents;GlobalBest=this.GlobalBest}

  type ParticleState =
    {
      Swarm : Agent<SwarmMsg>;
      Problem : OptimizationProblem;
      LocalBest : Solution;
      GlobalBest : Solution;
      Running : bool
    }
    with
    static member Create problem agent globalBest =
      {LocalBest=globalBest;GlobalBest=globalBest;Swarm=agent; Running = false; Problem=problem}
