using CodeStuff.TalkSuggestion.Views;
using Marten;
using Marten.Events.Projections;

namespace CodeStuff.TalkSuggestion;

public static class Configuration
{
    public static IServiceCollection AddSuggestions(this IServiceCollection services) =>
        services
            .AddScoped<SuggestionCommandHandler>()
            .AddTransient<GetAll<ActiveSuggestion>>(svc => svc.GetRequiredService<SuggestionData>().GetActiveSuggestions)
            .AddSingleton(SuggestionDecider.Decider)
            .AddSingleton<Evolver<Guid, Suggestion>>(SuggestionDecider.Decider)
            .AddScoped<SuggestionData>()
            .AddScoped<Loader<Guid, Suggestion>>(svc => svc.GetRequiredService<SuggestionData>().Load)
            .AddScoped<Saver<Guid, Suggestion>>(svc => svc.GetRequiredService<SuggestionData>().Save)
            .ConfigureMarten(config =>
            {
                config.Projections.Add<ActiveSuggestionProjection>(ProjectionLifecycle.Inline);

                config.Schema.For<ActiveSuggestion>().Identity(s => s.SuggestionId);
            })
    ;

}