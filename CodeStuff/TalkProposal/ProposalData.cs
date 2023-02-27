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

    public async Task<ProposalDetail?> GetDetail(Guid id)
    {
        await using var session = Store.QuerySession();
        return await session.Query<ProposalDetail>().SingleOrDefaultAsync(p => p.ProposalId == id);
    }
}