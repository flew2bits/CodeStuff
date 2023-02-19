using CodeStuff;
using CodeStuff.TalkProposal;
using CodeStuff.TalkProposal.Commands;
using CodeStuff.TalkProposal.Events;
using CodeStuff.TalkProposal.Views;
using FluentValidation;
using Marten;
using Marten.Events.Projections;
using Marten.Services.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Weasel.Core;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddTransient<IClaimsTransformation, ClaimsAdjuster>();
builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddOpenIdConnect(options => builder.Configuration.GetSection("Auth").Bind(options))
    .AddCookie();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddScoped<ProposalCommandHandler>();
builder.Services.AddTransient<QueryAll<ActiveProposal>>(svc => svc.GetRequiredService<ProposalData>().GetActiveProposals);
builder.Services.AddTransient<Query<Guid, ProposalDetail>>(svc => svc.GetRequiredService<ProposalData>().GetDetail);
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddSingleton(ProposalDecider.Decider);
builder.Services.AddSingleton<Evolver<Guid, Proposal>>(ProposalDecider.Decider);
builder.Services.AddScoped<ProposalData>();
builder.Services.AddScoped<Loader<Guid, Proposal>>(svc => svc.GetRequiredService<ProposalData>().Load);
builder.Services.AddScoped<Saver<Guid, Proposal>>(svc => svc.GetRequiredService<ProposalData>().Save);
builder.Services.AddMarten(config =>
{
    config.Connection(
        "User ID=postgres;Host=localhost;Port=5432;Database=postgres");
    config.UseDefaultSerialization(serializerType: SerializerType.SystemTextJson);
    config.AutoCreateSchemaObjects = AutoCreate.All;

    config.Projections.Add<ActiveProposalProjection>(ProjectionLifecycle.Inline);
    config.Projections.Add<ProposalDetailProjection>(ProjectionLifecycle.Inline);
});

var app = builder.Build();
app.UsePathBase("/CodeStuff");
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
// app.MapGet("/", () => "Hello World!");

app.MapPost("/api/Propose/Vote/{proposalId:guid}",
    [Authorize] async (HttpContext ctx, Guid proposalId, ProposalCommandHandler commandHandler) =>
    {
        try
        {
            var (_, events) = await commandHandler.HandleCommand(proposalId, new VoteForTalk(ctx.User.Identity!.Name!));
            if (!events.Any(e => e is VoteAdded)) return Results.BadRequest("User already has a vote");
        }
        catch
        {
            return Results.NotFound();
        }

        return Results.Accepted();
    });

app.Run();

public delegate Task<IEnumerable<TView>> QueryAll<TView>();

public delegate Task<TView> Query<in TIdentity, TView>(TIdentity id);