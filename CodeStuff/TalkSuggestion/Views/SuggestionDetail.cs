using CodeStuff.EntityShared.Events;
using CodeStuff.EntityShared.Views;
using CodeStuff.TalkSuggestion.Events;
using Marten.Events.Aggregation;

namespace CodeStuff.TalkSuggestion.Views;

public record SuggestionDetail(Guid SuggestionId, string Topic, string SuggestedBy, string AdditionalDetails, Comment[] Comments);

public class SuggestionDetailProjection : SingleStreamAggregation<SuggestionDetail>
{
    public SuggestionDetail Create(SuggestionSubmitted evt) =>
        new SuggestionDetail(evt.SuggestionId, evt.Topic, evt.SubmittedBy, evt.AdditionalDetails, Array.Empty<Comment>());

    public SuggestionDetail Apply(CommentThreadStarted evt, SuggestionDetail state) =>
        state with
        {
            Comments = state.Comments
                .Append(new Comment(evt.CommentId, evt.User, evt.Text, evt.TimeStamp, state.SuggestionId)).ToArray()
        };

    public SuggestionDetail Apply(ReplyAddedToComment evt, SuggestionDetail state) =>
        state with
        {
            Comments = state.Comments
                .Append(new Comment(evt.CommentId, evt.User, evt.Text, evt.TimeStamp, evt.InReplyTo)).ToArray()
        };
}