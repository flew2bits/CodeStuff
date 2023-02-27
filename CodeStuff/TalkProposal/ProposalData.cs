using CodeStuff.EntityShared.Views;
using CodeStuff.Infrastructure;
using CodeStuff.TalkProposal.Views;
using Marten;

namespace CodeStuff.TalkProposal;

public class ProposalData: MartenData<Proposal>
    
{
    public ProposalData(IDocumentStore store, Evolver<Guid, Proposal> evolver): base(store, evolver)
    {
    }

    public async Task<IEnumerable<ActiveProposal>> GetActiveProposals()
    {
        await using var session = Store.QuerySession();
        return await session.Query<ActiveProposal>().ToListAsync();
    }

    public async Task<ProposalDetailWithComments?> GetDetail(Guid id)
    {
        await using var session = Store.QuerySession();
        var detail = await session.Query<ProposalDetail>().SingleOrDefaultAsync(p => p.ProposalId == id);

        if (detail is null) return null;

        var comments = await session.Query<Comment>().Where(c => c.EntityId == id).ToListAsync();
        return new ProposalDetailWithComments(detail, comments.ToArray());
    }
}