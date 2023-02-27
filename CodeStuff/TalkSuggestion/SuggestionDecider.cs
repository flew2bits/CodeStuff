using CodeStuff.EntityShared;
using CodeStuff.EntityShared.Commands;
using CodeStuff.EntityShared.Events;
using CodeStuff.TalkSuggestion.Commands;
using CodeStuff.TalkSuggestion.Events;
using LamarCodeGeneration.Util;

namespace CodeStuff.TalkSuggestion;

public static class SuggestionDecider
{
    private static object[] Events(params object[] events) => events;
    private static object[] NoEvents => Array.Empty<object>();

    private static IEnumerable<object> Decide(Suggestion state, object command) =>
        command switch
        {
            SubmitSuggestion s => Events(new SuggestionSubmitted(state.SuggestionId, s.Topic, s.AdditionalDetails,
                s.Name)),
            StartCommentThread c => Events(new CommentThreadStarted(state.SuggestionId, Guid.NewGuid(), c.Text, c.User,
                DateTime.UtcNow)),
            ReplyToComment r => Events(new ReplyAddedToComment(state.SuggestionId, Guid.NewGuid(), r.Text, r.User,
                DateTime.UtcNow, r.InReplyTo)),
            _ => NoEvents
        };

    private static Suggestion Evolve(Suggestion state, object @event) =>
        @event switch
        {
            SuggestionSubmitted s => state with
            {
                Topic = s.Topic, AdditionalDetails = s.AdditionalDetails, SubmittedBy = s.SubmittedBy
            },
            CommentThreadStarted t => state with
            {
                Comments = state.Comments
                    .Append(new EntityComment(t.CommentId, t.Text, t.User, t.TimeStamp, state.SuggestionId))
                    .ToArray()
            },
            ReplyAddedToComment r => state with
            {
                Comments = state.Comments
                    .Append(new EntityComment(r.CommentId, r.Text, r.User, r.TimeStamp, r.InReplyTo)).ToArray()
            },
            _ => state
        };

    private static Suggestion InitialState(Guid id) => new(id, "", "", "", Array.Empty<EntityComment>());

    private static bool IsCreator(object cmd) => cmd is SubmitSuggestion;

    private static bool IsFinal(Suggestion _) => false;

    public static Decider<Guid, Suggestion> Decider = new(Decide, Evolve, InitialState, IsFinal, IsCreator);
}