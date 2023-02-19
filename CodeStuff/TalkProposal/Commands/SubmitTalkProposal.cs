namespace CodeStuff.TalkProposal.Commands;

public record SubmitTalkProposal(string Title, string Brief, string User, DateOnly ReadyDate);