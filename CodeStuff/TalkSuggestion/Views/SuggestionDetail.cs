using CodeStuff.EntityShared.Events;
using CodeStuff.EntityShared.Views;
using CodeStuff.TalkSuggestion.Events;
using Marten.Events.Aggregation;
using Marten.Events.Projections;

namespace CodeStuff.TalkSuggestion.Views;

public record SuggestionDetail(Guid SuggestionId, string Topic, string SuggestedBy, string AdditionalDetails);

public record SuggestionDetailWithComments(SuggestionDetail Detail, Comment[] Comments);

public class SuggestionDetailProjection : SingleStreamAggregation<SuggestionDetail>
{
    public SuggestionDetail Create(SuggestionSubmitted evt) =>
        new SuggestionDetail(evt.SuggestionId, evt.Topic, evt.SubmittedBy, evt.AdditionalDetails);

}