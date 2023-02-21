using CodeStuff.TalkProposal.Views;
using Marten;
using Marten.Events.Projections;

namespace CodeStuff.TalkProposal;

public static class Configuration
{
    public static IServiceCollection AddProposals(this IServiceCollection services) =>
        services
            .AddScoped<ProposalCommandHandler>()
            .AddTransient<GetAll<ActiveProposal>>(svc => svc.GetRequiredService<ProposalData>().GetActiveProposals)
            .AddTransient<Find<Guid, ProposalDetail>>(svc => svc.GetRequiredService<ProposalData>().GetDetail)
            .AddSingleton(ProposalDecider.Decider)
            .AddSingleton<Evolver<Guid, Proposal>>(ProposalDecider.Decider)
            .AddScoped<ProposalData>()
            .AddScoped<Loader<Guid, Proposal>>(svc => svc.GetRequiredService<ProposalData>().Load)
            .AddScoped<Saver<Guid, Proposal>>(svc => svc.GetRequiredService<ProposalData>().Save)
            .ConfigureMarten(config =>
            {
                config.Projections.Add<ActiveProposalProjection>(ProjectionLifecycle.Inline);
                config.Projections.Add<ProposalDetailProjection>(ProjectionLifecycle.Inline);

                config.Schema.For<ActiveProposal>().Identity(p => p.ProposalId);
                config.Schema.For<ProposalDetail>().Identity(p => p.ProposalId);
            });
}