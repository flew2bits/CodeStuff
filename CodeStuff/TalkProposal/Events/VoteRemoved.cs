namespace CodeStuff.TalkProposal.Events;

public record VoteRemoved(Guid ProposalId, string UserName, DateTime TimeStamp);