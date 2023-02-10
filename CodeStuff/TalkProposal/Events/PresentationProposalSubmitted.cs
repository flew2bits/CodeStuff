namespace CodeStuff.TalkProposal.Events;

public record PresentationProposalSubmitted(Guid ProposalId, string Title, string Brief, string User);