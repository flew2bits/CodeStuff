using System.Security.Claims;
using CodeStuff.TalkProposal;
using CodeStuff.TalkProposal.Commands;
using CodeStuff.TalkProposal.Views;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeStuff.Pages.Propose;

[Authorize]
public class Index : PageModel
{
    public ICollection<ActiveProposal> ActiveProposals { get; private set; } = Array.Empty<ActiveProposal>();

    [BindProperty]
    public ProposeTalkRequest TalkProposal { get; set; } = new(null, null, null);
    
    public async Task OnGet([FromServices] GetAll<ActiveProposal> getActiveProposals)
    {
        ActiveProposals = (await getActiveProposals()).ToArray();
    }

    public async Task<IActionResult> OnPostProposeTalk(
        [FromServices] ProposalCommandHandler commandHandler,
        [FromServices] IValidator<ProposeTalkRequest> validator)
    {
        var result = await validator.ValidateAsync(TalkProposal);
        
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            
            TempData["Errors"] =
                string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return RedirectToPage();
        }

        var presenterName =
            $"{HttpContext.User.FindFirstValue(ClaimTypes.GivenName)} {HttpContext.User.FindFirstValue(ClaimTypes.Surname)}";
        
        await commandHandler.HandleCommand(Guid.NewGuid(), new SubmitTalkProposal(TalkProposal.Title!, TalkProposal.Brief!,
            presenterName, TalkProposal.ReadyDate!.Value));

        return RedirectToPage();
    }

    public record ProposeTalkRequest(string? Title, string? Brief, DateOnly? ReadyDate);

    public class ProposeTalkRequestValidator : AbstractValidator<ProposeTalkRequest>
    {
        public ProposeTalkRequestValidator()
        {
            RuleFor(p => p.Title).NotEmpty();
            RuleFor(p => p.Brief).NotEmpty();
            RuleFor(p => p.ReadyDate).NotEmpty().GreaterThan(DateOnly.FromDateTime(DateTime.Today));
        }
    }
}