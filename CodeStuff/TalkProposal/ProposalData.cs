using CodeStuff.TalkProposal.Views;
using Marten;

namespace CodeStuff.TalkProposal;

public class ProposalData
{
    private readonly IDocumentStore _store;
    private readonly Evolver<Guid, Proposal> _evolver;

    public ProposalData(IDocumentStore store, Evolver<Guid, Proposal> evolver)
    {
        _store = store;
        _evolver = evolver;
    }

    public async Task<Proposal> Load(Guid id)
    {
        await using var session = _store.QuerySession();
        var events = await session.Events.FetchStreamAsync(id);
        if (!events.Any()) throw new InvalidOperationException("Entity does not exist");
        return events.Select(e => e.Data).Aggregate(_evolver.InitialState(id), _evolver.Evolve);
    }

    public async Task<bool> Save(Guid id, Proposal _, IEnumerable<object> events)
    {
        await using var session = _store.LightweightSession();
        session.Events.Append(id, events);
        await session.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<ActiveProposal>> GetActiveProposals()
    {
        await using var session = _store.QuerySession();
        return await session.Query<ActiveProposal>().ToListAsync();
    }

    public async Task<ProposalDetail?> GetDetail(Guid id)
    {
        await using var session = _store.QuerySession();
        return await session.Query<ProposalDetail>().SingleOrDefaultAsync(p => p.Id == id);
    }
}