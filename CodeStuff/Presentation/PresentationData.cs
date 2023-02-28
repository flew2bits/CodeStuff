using CodeStuff.Infrastructure;
using CodeStuff.Presentation.Views;
using Marten;

namespace CodeStuff.Presentation;

public class PresentationData: MartenData<Presentation>
{
    public PresentationData(IDocumentStore store, Evolver<Guid, Presentation> evolver) : base(store, evolver)
    {
    }

    public async Task<IEnumerable<PresentationListItem>> GetAllPresentations()
    {
        await using var session = Store.QuerySession();
        return await session.Query<PresentationListItem>().ToListAsync();
    }
}
