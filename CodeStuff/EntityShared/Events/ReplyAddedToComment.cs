namespace CodeStuff.EntityShared.Events;

public record ReplyAddedToComment(Guid EntityId, Guid CommentId, string Text, string User, DateTime TimeStamp,
    Guid InReplyTo): ICommentEvent;
