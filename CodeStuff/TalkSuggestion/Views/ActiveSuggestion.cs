using CodeStuff.TalkSuggestion.Events;
using Marten.Events.Aggregation;

namespace CodeStuff.TalkSuggestion.Views;

public record ActiveSuggestion(Guid SuggestionId, string Topic, string SuggestedBy);

public class ActiveSuggestionProjection : SingleStreamAggregation<ActiveSuggestion>
{
    public ActiveSuggestion Create(SuggestionSubmitted evt) => new(evt.SuggestionId, evt.Topic, evt.SubmittedBy);
}