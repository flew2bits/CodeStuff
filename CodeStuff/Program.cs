using CodeStuff.TalkProposal;
using CodeStuff.TalkProposal.Commands;
using CodeStuff.TalkProposal.Events;
using CodeStuff.TalkProposal.Views;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;

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
builder.Services.AddTransient<QueryAll<ActiveProposal>>(_ => ProposalCommandHandler.QueryActive);
builder.Services.AddTransient<Query<Guid, ProposalDetail>>(_ => ProposalCommandHandler.QueryDetail);
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

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