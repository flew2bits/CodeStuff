using CodeStuff.EntityShared;

namespace CodeStuff.TalkSuggestion;

public record Suggestion(Guid SuggestionId, string Topic, string AdditionalDetails, string SubmittedBy, EntityComment[] Comments);