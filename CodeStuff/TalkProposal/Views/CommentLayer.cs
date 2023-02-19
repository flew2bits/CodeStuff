namespace CodeStuff.TalkProposal.Views;


public record Comment(Guid CommentId, string User, string Text, DateTime TimeStamp, Guid InReplyTo);