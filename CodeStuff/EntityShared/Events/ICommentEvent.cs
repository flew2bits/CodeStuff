namespace CodeStuff.EntityShared.Events;

public interface ICommentEvent
{
    Guid EntityId { get; }
    Guid CommentId { get; }
}