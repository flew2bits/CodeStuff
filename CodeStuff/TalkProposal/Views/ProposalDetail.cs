using CodeStuff.TalkProposal.Events;
using Marten.Events.Aggregation;

namespace CodeStuff.TalkProposal.Views;

public record ProposalDetail(Guid ProposalId, string Title, string Brief, string Presenter, DateOnly ReadyDate,
    Comment[] Comments);

public record Comment(Guid CommentId, string User, string Text, DateTime TimeStamp, Guid InReplyTo);

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ProposalDetailProjection : SingleStreamAggregation<ProposalDetail>
{
    public ProposalDetail Create(TalkProposalSubmitted evt) =>
        new(evt.ProposalId, evt.Title, evt.Brief, evt.User, evt.ReadyDate, Array.Empty<Comment>());

    public ProposalDetail Apply(CommentThreadStarted comment, ProposalDetail proposalDetail) =>
        proposalDetail with
        {
            Comments = proposalDetail.Comments.Append(new Comment(comment.CommentId, comment.User, comment.Text,
                comment.TimeStamp, proposalDetail.ProposalId)).ToArray()
        };

    public ProposalDetail Apply(ReplyAddedToProposalComment reply, ProposalDetail proposalDetail) =>
        proposalDetail with
        {
            Comments = proposalDetail.Comments
                .Append(new Comment(reply.CommentId, reply.User, reply.Text, reply.TimeStamp, reply.InReplyTo))
                .ToArray()
        };
}

