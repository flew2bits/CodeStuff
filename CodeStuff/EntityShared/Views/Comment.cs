using System.Globalization;
using CodeStuff.EntityShared.Events;
using Marten.Events.Aggregation;
using Marten.Events.Projections;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CodeStuff.EntityShared.Views;

public record Comment(Guid CommentId, Guid EntityId, string User, string Text, DateTime TimeStamp, Guid InReplyTo);

public class CommentsProjection : MultiStreamAggregation<Comment, Guid>
{
    public CommentsProjection()
    {
        Identity<ICommentEvent>(c => c.CommentId);
    }
    
    public Comment Create(CommentThreadStarted comment) => new(comment.CommentId, comment.EntityId, comment.User,
        comment.Text, comment.TimeStamp, comment.EntityId);

    public Comment Create(ReplyAddedToComment reply) => new(reply.CommentId, reply.EntityId, reply.User, reply.Text,
        reply.TimeStamp, reply.InReplyTo);
}