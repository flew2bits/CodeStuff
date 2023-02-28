using CodeStuff.Presentation.Events;
using Marten.Events.Aggregation;

namespace CodeStuff.Presentation.Views;

public record PresentationListItem(Guid PresentationId, string Title, string Presenter, DateTime ScheduledTime, TimeSpan ScheduledDuration);

public class PresentationListItemProjection : SingleStreamAggregation<PresentationListItem>
{
    public PresentationListItem Create(PresentationScheduled evt) => new(evt.PresentationId, evt.Title, evt.Presenter,
        evt.ScheduledTime, evt.Duration);
}