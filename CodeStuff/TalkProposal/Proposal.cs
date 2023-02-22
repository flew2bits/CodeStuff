using CodeStuff.EntityShared;

namespace CodeStuff.TalkProposal;

public record Proposal(Guid Id, string Title, string Brief, string Presenter, DateOnly ReadyDate, ProposalVote[] Votes, EntityComment[] Comments);

public record ProposalVote(string User, DateTime TimeStamp);

