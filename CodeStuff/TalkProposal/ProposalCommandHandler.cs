using CodeStuff.TalkProposal.Views;

namespace CodeStuff.TalkProposal;

public record ProposalCommandHandler() :
    EntityCommandHandler<Guid, Proposal>(ProposalDecider.Decider,
        id => Task.FromResult(Database[id]),
        Savers
    )
{
    private static readonly Dictionary<Guid, Proposal> Database = new();

    private static readonly IEnumerable<Saver<Guid, Proposal>> Savers = new[] { (Saver<Guid, Proposal>)Save };

    private static Task<bool> Save(Guid id, Proposal entity, IEnumerable<object> _)
    {
        Database[id] = entity;
        return Task.FromResult(true);
    }

    public static Task<IEnumerable<ActiveProposal>> QueryActive() =>
        Task.FromResult(Database.Values.Select(p =>
            new ActiveProposal(p.Id, p.Title, p.Presenter, p.Votes.Select(v => v.User).ToArray())));

    private static CommentLayer BuildCommentTree(IDictionary<Guid, ProposalComment[]> allComments,
        Guid start)
        => allComments.TryGetValue(start, out var comments)
            ? new CommentLayer(comments.Select(c => new Comment(c.CommentId, c.User, c.Text, c.TimeStamp.ToLocalTime(),
                BuildCommentTree(allComments, c.CommentId))).ToArray())
            : CommentLayer.Empty;

    public static Task<ProposalDetail> QueryDetail(Guid proposalId)
    {
        if (!Database.TryGetValue(proposalId, out var proposal))
            throw new InvalidOperationException("Proposal does not exist");

        var comments = proposal.Comments
            .GroupBy(c => c.InReplyTo).ToDictionary(k => k.Key, v => v.ToArray());

        return Task.FromResult(new ProposalDetail(proposal.Title, proposal.Brief, proposal.Presenter, proposal.ReadyDate,
            BuildCommentTree(comments, proposalId)));
    }
}