using CodeStuff.TalkProposal.Commands;

namespace CodeStuff.TalkProposal;

public static class ProposalDecider
{

    private static object[] Events(params object[] events) => events;
    private static object[] NoEvents => Array.Empty<object>();

    private static object[] Decide(Proposal state, object command) => NoEvents;

    private static Proposal Evolve(Proposal state, object @event) => state;

    private static Proposal InitialState(Guid id) => new(id);
    
    private static bool IsTerminal(Proposal _) => false;

    private static bool IsCreator(object c) => c is SubmitPresentationProposal;

    public static readonly Decider<Guid, Proposal> Decider = new(Decide, Evolve, InitialState, IsTerminal, IsCreator);
};