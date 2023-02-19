using CodeStuff.TalkProposal.Events;
using Marten.Events.Aggregation;

namespace CodeStuff.TalkProposal.Views;

public record ActiveProposal(Guid Id, string Title, string Presenter, string[] Voters);

public class ActiveProposalProjection : SingleStreamAggregation<ActiveProposal>
{
    public ActiveProposal Apply(VoteAdded evt, ActiveProposal activeProposal) =>
        activeProposal with { Voters = activeProposal.Voters.Append(evt.User).ToArray() };

    public ActiveProposal Apply(VoteRemoved evt, ActiveProposal activeProposal) =>
        activeProposal with { Voters = activeProposal.Voters.Where(v => v != evt.UserName).ToArray() };
    
    public ActiveProposal Create(TalkProposalSubmitted evt) =>
        new ActiveProposal(evt.ProposalId, evt.Title, evt.User, Array.Empty<string>());
}