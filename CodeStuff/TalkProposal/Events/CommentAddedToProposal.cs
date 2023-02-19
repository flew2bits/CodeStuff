namespace CodeStuff.TalkProposal.Events;

public record CommentAddedToProposal(Guid ProposalId, Guid CommentId, string User, string Text, DateTime TimeStamp);