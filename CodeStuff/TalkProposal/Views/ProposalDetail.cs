using CodeStuff.TalkProposal.Events;
using Marten.Events.Aggregation;

namespace CodeStuff.TalkProposal.Views;

public record ProposalDetail(Guid Id, string Title, string Brief, string Presenter, DateOnly ReadyDate,
    Comment[] Comments);

public class ProposalDetailProjection : SingleStreamAggregation<ProposalDetail>
{
    public ProposalDetail Create(TalkProposalSubmitted evt) =>
        new(evt.ProposalId, evt.Title, evt.Brief, evt.User, evt.ReadyDate, Array.Empty<Comment>());

    public ProposalDetail Apply(CommentAddedToProposal comment, ProposalDetail proposalDetail) =>
        proposalDetail with
        {
            Comments = proposalDetail.Comments.Append(new Comment(comment.CommentId, comment.User, comment.Text,
                comment.TimeStamp, proposalDetail.Id)).ToArray()
        };

    public ProposalDetail Apply(ReplyAddedToProposalComment reply, ProposalDetail proposalDetail) =>
        proposalDetail with
        {
            Comments = proposalDetail.Comments
                .Append(new Comment(reply.CommentId, reply.User, reply.Text, reply.TimeStamp, reply.InReplyTo))
                .ToArray()
        };
}