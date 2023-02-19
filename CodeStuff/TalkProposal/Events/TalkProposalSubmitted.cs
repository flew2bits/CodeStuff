namespace CodeStuff.TalkProposal.Events;

public record TalkProposalSubmitted(Guid ProposalId, string Title, string Brief, string User, DateOnly ReadyDate);