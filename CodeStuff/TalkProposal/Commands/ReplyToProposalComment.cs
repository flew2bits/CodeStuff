namespace CodeStuff.TalkProposal.Commands;

public record ReplyToProposalComment(string User, string Text, Guid InReplyTo);
