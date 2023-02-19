using CodeStuff.TalkProposal;
using CodeStuff.TalkProposal.Commands;
using CodeStuff.TalkProposal.Events;
using FluentValidation;
using Marten;
using Marten.Services.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Weasel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddOpenIdConnect(options => builder.Configuration.GetSection("Auth").Bind(options))
    .AddCookie();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddMarten(config =>
{
    config.Connection(
        "User ID=postgres;Host=localhost;Port=5432;Database=postgres");
    config.UseDefaultSerialization(serializerType: SerializerType.SystemTextJson);
    config.AutoCreateSchemaObjects = AutoCreate.All;
});
builder.Services.AddProposals();

var app = builder.Build();
app.UsePathBase("/CodeStuff");
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
// app.MapGet("/", () => "Hello World!");

app.MapPost("/api/Propose/Vote/{id:guid}",
    [Authorize] async (HttpContext ctx, Guid id, ProposalCommandHandler commandHandler) =>
    {
        try
        {
            var (_, events) = await commandHandler.HandleCommand(id, new ToggleVoteForProposal(ctx.User.Identity!.Name!));
            if (!events.Any(e => e is VoteAdded or VoteRemoved)) return Results.BadRequest("User can not change vote");
        }
        catch
        {
            return Results.NotFound();
        }

        return Results.Accepted();
    }).WithName("VoteProposal");

app.Run();

public delegate Task<IEnumerable<TView>> QueryAll<TView>();

public delegate Task<TView> Query<in TIdentity, TView>(TIdentity id);