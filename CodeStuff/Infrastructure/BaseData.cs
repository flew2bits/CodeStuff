using Marten;

namespace CodeStuff.Infrastructure;

public abstract class BaseData<TEntity> where TEntity:class
{
    protected readonly IDocumentStore _store;
    private readonly Evolver<Guid, TEntity> _evolver;

    protected BaseData(IDocumentStore store, Evolver<Guid, TEntity> evolver)
    {
        _store = store;
        _evolver = evolver;
    }

    public async Task<TEntity> Load(Guid id)
    {
        await using var session = _store.QuerySession();
        var events = await session.Events.FetchStreamAsync(id);
        if (!events.Any()) throw new InvalidOperationException("Entity does not exist");
        return events.Select(e => e.Data).Aggregate(_evolver.InitialState(id), _evolver.Evolve);
    }

    public async Task<bool> Save(Guid id, TEntity _, IEnumerable<object> events)
    {
        await using var session = _store.LightweightSession();
        session.Events.Append(id, events);
        await session.SaveChangesAsync();
        return true;
    }
}