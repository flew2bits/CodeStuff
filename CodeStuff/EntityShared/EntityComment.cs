namespace CodeStuff.EntityShared;

public record EntityComment(Guid CommentId, string Text, string User, DateTime TimeStamp, Guid InReplyTo);
