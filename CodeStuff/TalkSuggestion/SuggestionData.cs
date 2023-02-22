using CodeStuff.Infrastructure;
using CodeStuff.TalkSuggestion.Views;
using Marten;

namespace CodeStuff.TalkSuggestion;

public class SuggestionData: BaseData<Suggestion>
{
    public SuggestionData(IDocumentStore store, Evolver<Guid, Suggestion> evolver) : base(store, evolver)
    {
    }

    public async Task<IEnumerable<ActiveSuggestion>> GetActiveSuggestions()
    {
        await using var session = _store.QuerySession();
        return await session.Query<ActiveSuggestion>().ToListAsync();
    }
}