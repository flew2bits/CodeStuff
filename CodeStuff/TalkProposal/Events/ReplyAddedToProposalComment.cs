namespace CodeStuff.TalkProposal.Events;

public record ReplyAddedToProposalComment(Guid ProposalId, Guid CommentId, string User, string Text, DateTime TimeStamp, Guid InReplyTo);
