using System.Security.Claims;
using CodeStuff.TalkProposal;
using CodeStuff.TalkProposal.Commands;
using CodeStuff.TalkProposal.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeStuff.Pages.Propose;

[Authorize]
public class Details : PageModel
{
    [BindProperty(SupportsGet = true)] public Guid ProposalId { get; set; } = Guid.Empty;

    public ProposalDetail? Detail { get; set; }

    public async Task<IActionResult> OnGet([FromServices] Find<Guid, ProposalDetail?> findProposalDetail)
    {
        Detail = await findProposalDetail(ProposalId);
        if (Detail is null) return NotFound();

        return Page();
    }

    public async Task<IActionResult> OnPostAddComment(string text, Guid? inReplyTo,
        [FromServices] ProposalCommandHandler commandHandler)
    {
        await commandHandler.HandleCommand(ProposalId, inReplyTo.HasValue
            ? new ReplyToProposalComment(User.FindFirstValue(ClaimTypes.Upn)!, text, inReplyTo.Value)
            : new AddCommentToProposal(User.FindFirstValue(ClaimTypes.Upn)!, text));
        return RedirectToPage(new { ProposalId });
    }
}