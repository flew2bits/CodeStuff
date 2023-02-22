namespace CodeStuff.EntityShared.Commands;

public record ReplyToComment(string Text, string User, Guid InReplyTo);