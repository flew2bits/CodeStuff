namespace CodeStuff.TalkSuggestion.Events;

public record SuggestionSubmitted(Guid SuggestionId, string Topic, string AdditionalDetails, string SubmittedBy);