using CodeStuff.TalkSuggestion.Commands;
using CodeStuff.TalkSuggestion.Events;

namespace CodeStuff.TalkSuggestion;

public static class SuggestionDecider
{
    private static object[] Events(params object[] events) => events;
    private static object[] NoEvents => Array.Empty<object>();

    private static IEnumerable<object> Decide(Suggestion state, object command) =>
        command switch
        {
            SubmitSuggestion s => Events(new SuggestionSubmitted(state.SuggestionId, s.Topic, s.AdditionalDetails)),
            _ => NoEvents
        };

    private static Suggestion Evolve(Suggestion state, object @event) => state;

    private static Suggestion InitialState(Guid id) => new(id);

    private static bool IsCreator(object cmd) => cmd is SubmitSuggestion;

    private static bool IsFinal(Suggestion _) => false;
    
    public static Decider<Guid, Suggestion> Decider = new(Decide, Evolve, InitialState, IsFinal, IsCreator);
}