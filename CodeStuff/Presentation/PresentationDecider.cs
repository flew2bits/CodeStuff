using CodeStuff.Presentation.Commands;
using CodeStuff.Presentation.Events;

namespace CodeStuff.Presentation;

public static class PresentationDecider
{
    private static object[] Events(params object[] events) => events;
    private static object[] NoEvents => Array.Empty<object>();

    private static IEnumerable<object> Decide(Presentation state, object command) =>
        command switch
        {
            SchedulePresentation => Events(new PresentationScheduled()),
            _ => NoEvents
        };
    
    private static Presentation Evolve(Presentation state, object @event) => state;

    private static Presentation InitialState(Guid id) => new();

    private static bool IsCreator(object command) => command is SchedulePresentation;

    private static bool IsFinal(Presentation state) => false;

    public static readonly Decider<Guid, Presentation> Decider = new(Decide, Evolve, InitialState, IsFinal, IsCreator);
}