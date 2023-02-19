namespace CodeStuff.TalkProposal.Events;

public record VoteAdded(Guid ProposalId, string User, DateTime TimeStamp);