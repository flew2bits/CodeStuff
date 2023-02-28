namespace CodeStuff.Presentation;

public static class Configuration
{
    public static IServiceCollection AddPresentation(this IServiceCollection services) =>
        services
            .AddScoped<PresentationData>()
            .AddScoped<Loader<Guid, Presentation>>(svc => svc.GetRequiredService<PresentationData>().Load)
            .AddScoped<Saver<Guid, Presentation>>(svc => svc.GetRequiredService<PresentationData>().Save)
            .AddSingleton(PresentationDecider.Decider)
            .AddSingleton<Evolver<Guid, Presentation>>(PresentationDecider.Decider);

}