namespace CodeStuff.Presentation.Events;

public record PresentationScheduled(Guid PresentationId, string Title, string Presenter, DateTime ScheduledTime, TimeSpan Duration);