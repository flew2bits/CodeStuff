using CodeStuff.EntityShared;
using CodeStuff.EntityShared.Commands;
using CodeStuff.EntityShared.Events;
using CodeStuff.TalkProposal.Commands;
using CodeStuff.TalkProposal.Events;

namespace CodeStuff.TalkProposal;

public static class ProposalDecider
{
    private static object[] Events(params object[] events) => events;
    private static object[] NoEvents => Array.Empty<object>();

    private static object[] Decide(Proposal state, object command) =>
        command switch
        {
            SubmitTalkProposal p => Events(new TalkProposalSubmitted(state.Id, p.Title, p.Brief, p.User, p.ReadyDate)),
            ToggleVoteForProposal toggle => state.Votes.Any(v => v.User == toggle.UserName)
                ? Events(new VoteRemoved(state.Id, toggle.UserName, DateTime.UtcNow))
                : Events(new VoteAdded(state.Id, toggle.UserName, DateTime.UtcNow)),
            StartCommentThread c => Events(new CommentThreadStarted(state.Id, Guid.NewGuid(), c.User, c.Text,
                DateTime.UtcNow)),
            ReplyToComment r => state.Comments.Any(c => c.CommentId == r.InReplyTo)
                ? Events(new ReplyAddedToComment(state.Id, Guid.NewGuid(), r.User, r.Text, DateTime.UtcNow,
                    r.InReplyTo))
                : NoEvents,
            _ => NoEvents
        };

    private static Proposal Evolve(Proposal state, object @event) =>
        @event switch
        {
            TalkProposalSubmitted ps =>
                state with { Title = ps.Title, Brief = ps.Brief, Presenter = ps.User, ReadyDate = ps.ReadyDate},
            VoteAdded vt => state with
            {
                Votes = state.Votes.Append(new ProposalVote(vt.User, vt.TimeStamp)).ToArray()
            },
            VoteRemoved uvt => state with
            {
                Votes = state.Votes.Where(v => v.User != uvt.UserName).ToArray()
            },
            CommentThreadStarted c => state with
            {
                Comments = state.Comments.Append(new EntityComment(c.CommentId, c.User, c.Text, c.TimeStamp, state.Id)).ToArray()
            },
            ReplyAddedToComment r => state with
            {
                Comments = state.Comments.Append(new EntityComment(r.CommentId, r.User, r.Text, r.TimeStamp, r.InReplyTo)).ToArray()
            },
            _ => state
        };

    private static Proposal InitialState(Guid id) => new(id, "", "", "None", DateOnly.MinValue, Array.Empty<ProposalVote>(), Array.Empty<EntityComment>());

    private static bool IsTerminal(Proposal _) => false;

    private static bool IsCreator(object c) => c is SubmitTalkProposal;

    public static readonly Decider<Guid, Proposal> Decider = new(Decide, Evolve, InitialState, IsTerminal, IsCreator);
};