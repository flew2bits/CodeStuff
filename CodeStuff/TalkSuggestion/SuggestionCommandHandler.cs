namespace CodeStuff.TalkSuggestion;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record SuggestionCommandHandler(Loader<Guid, Suggestion> Load, IEnumerable<Saver<Guid, Suggestion>> Save) :
    EntityCommandHandler<Guid, Suggestion>(SuggestionDecider.Decider, Load, Save);
