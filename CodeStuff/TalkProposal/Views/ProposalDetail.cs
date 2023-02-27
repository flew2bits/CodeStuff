using CodeStuff.EntityShared.Events;
using CodeStuff.EntityShared.Views;
using CodeStuff.TalkProposal.Events;
using Marten.Events.Aggregation;
using Marten.Events.Projections;

namespace CodeStuff.TalkProposal.Views;

public record ProposalDetail(Guid ProposalId, string Title, string Brief, string Presenter, DateOnly ReadyDate);

public record ProposalDetailWithComments(ProposalDetail Detail, Comment[] Comments);

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ProposalDetailProjection : SingleStreamAggregation<ProposalDetail>
{
    public ProposalDetail Create(TalkProposalSubmitted evt) =>
        new(evt.ProposalId, evt.Title, evt.Brief, evt.User, evt.ReadyDate);

}

