namespace CodeStuff.TalkProposal.Views;

public record CommentLayer(Comment[] Comments)
{
    public static CommentLayer Empty => new(Array.Empty<Comment>());
}

public record Comment(Guid CommentId, string User, string Text, DateTime TimeStamp, CommentLayer SubCommentLayer);