namespace CodeStuff.Presentation;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public record PresentationCommandHandler(Loader<Guid, Presentation> Loader,
    IEnumerable<Saver<Guid, Presentation>> Savers) : EntityCommandHandler<Guid, Presentation>(
    PresentationDecider.Decider, Loader, Savers);