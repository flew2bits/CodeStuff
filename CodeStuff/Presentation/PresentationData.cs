using CodeStuff.Infrastructure;
using Marten;

namespace CodeStuff.Presentation;

public class PresentationData: MartenData<Presentation>
{
    public PresentationData(IDocumentStore store, Evolver<Guid, Presentation> evolver) : base(store, evolver)
    {
    }
}