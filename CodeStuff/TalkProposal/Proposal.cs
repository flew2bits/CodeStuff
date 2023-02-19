namespace CodeStuff.TalkProposal;

public record Proposal(Guid Id, string Title, string Brief, string Presenter, DateOnly ReadyDate, ProposalVote[] Votes, ProposalComment[] Comments);

public record ProposalVote(string User, DateTime TimeStamp);

public record ProposalComment(Guid CommentId, string User, string Text, DateTime TimeStamp, Guid InReplyTo);