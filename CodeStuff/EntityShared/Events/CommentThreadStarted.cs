namespace CodeStuff.EntityShared.Events;

public record CommentThreadStarted(Guid EntityId, Guid CommentId, string Text, string User, DateTime TimeStamp);
