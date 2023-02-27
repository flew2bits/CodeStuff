using CodeStuff.EntityShared.Views;
using Marten;
using Marten.Events.Projections;

namespace CodeStuff.EntityShared;

public static class Configuration
{
    public static IServiceCollection AddComments(this IServiceCollection services) =>
        services.ConfigureMarten(config =>
        {
            config.Projections.Add<CommentsProjection>(ProjectionLifecycle.Inline);
            config.Schema.For<Comment>().Identity(c => c.CommentId);
        });
}