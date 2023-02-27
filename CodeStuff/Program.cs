global using JetBrains.Annotations;
using CodeStuff.EntityShared;
using CodeStuff.TalkProposal;
using CodeStuff.TalkProposal.Commands;
using CodeStuff.TalkProposal.Events;
using CodeStuff.TalkSuggestion;
using FluentValidation;
using Marten;
using Marten.Services.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Weasel.Core;
using static Microsoft.AspNetCore.Http.Results;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.Configure<ForwardedHeadersOptions>(opts =>
// {
//     opts.ForwardedHeaders = ForwardedHeaders.All;
//     opts.KnownNetworks.Clear();
//     opts.KnownProxies.Clear();
// });
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
    config.Connection(builder.Configuration.GetConnectionString("Marten") ?? throw new InvalidOperationException());
    config.UseDefaultSerialization(serializerType: SerializerType.SystemTextJson);
    config.AutoCreateSchemaObjects = AutoCreate.All;
});
builder.Services
    .AddProposals()
    .AddSuggestions()
    .AddComments();

var app = builder.Build();
app.UseStaticFiles();
var headerOptions = new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.All };
headerOptions.KnownNetworks.Clear();
headerOptions.KnownProxies.Clear();
app.UseForwardedHeaders(headerOptions);
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
            var (state, events) =
                await commandHandler.HandleCommand(id, new ToggleVoteForProposal(ctx.User.Identity!.Name!));
            return events.Any(e => e is VoteAdded or VoteRemoved)
                ? Json(new { Votes = state.Votes.Length }, statusCode: StatusCodes.Status202Accepted)
                : BadRequest("User can not change vote");
        }
        catch
        {
            return NotFound();
        }
    }).WithName("VoteProposal");

app.Run();