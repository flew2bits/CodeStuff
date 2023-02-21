namespace CodeStuff.TalkProposal.Events;

public record CommentThreadStarted(Guid ProposalId, Guid CommentId, string User, string Text, DateTime TimeStamp);