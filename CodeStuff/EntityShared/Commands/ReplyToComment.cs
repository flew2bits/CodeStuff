namespace CodeStuff.EntityShared.Commands;

public record ReplyToComment(string User, string Text, Guid InReplyTo);