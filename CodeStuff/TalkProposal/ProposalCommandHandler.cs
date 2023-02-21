namespace CodeStuff.TalkProposal;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record ProposalCommandHandler(Loader<Guid, Proposal> Load, IEnumerable<Saver<Guid, Proposal>> Save) :
    EntityCommandHandler<Guid, Proposal>(ProposalDecider.Decider,
        Load,
        Save
    );
