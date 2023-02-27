using CodeStuff.EntityShared.Views;
using CodeStuff.Infrastructure;
using CodeStuff.TalkSuggestion.Views;
using Marten;
using Marten.Linq;

namespace CodeStuff.TalkSuggestion;

public class SuggestionData: MartenData<Suggestion>
{
    public SuggestionData(IDocumentStore store, Evolver<Guid, Suggestion> evolver) : base(store, evolver)
    {
    }

    public async Task<IEnumerable<ActiveSuggestion>> GetActiveSuggestions()
    {
        await using var session = Store.QuerySession();
        return await session.Query<ActiveSuggestion>().ToListAsync();
    }

    public async Task<SuggestionDetailWithComments?> FindSuggestionDetail(Guid suggestionId)
    {
        await using var session = Store.QuerySession();
        var detail = await session.Query<SuggestionDetail>().SingleOrDefaultAsync(s => s.SuggestionId == suggestionId);
        if (detail is null) return null;
        var comments = await session.Query<Comment>().Where(c => c.EntityId == suggestionId).ToListAsync();
        return new SuggestionDetailWithComments(detail, comments.ToArray());
    }
}