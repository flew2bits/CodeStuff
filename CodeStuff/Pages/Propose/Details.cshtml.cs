using System.Security.Claims;
using CodeStuff.EntityShared.Commands;
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

    public ProposalDetailWithComments Detail { get; set; } = null!;

    public async Task<IActionResult> OnGet([FromServices] Find<Guid, ProposalDetailWithComments?> findProposalDetail)
    {
        var detail = await findProposalDetail(ProposalId);
        if (detail is null) return NotFound();

        Detail = detail;

        return Page();
    }

    public async Task<IActionResult> OnPostAddComment(string text, Guid? inReplyTo,
        [FromServices] ProposalCommandHandler commandHandler)
    {
        await commandHandler.HandleCommand(ProposalId, inReplyTo.HasValue
            ? new ReplyToComment(User.FindFirstValue(ClaimTypes.Upn)!, text, inReplyTo.Value)
            : new StartCommentThread(User.FindFirstValue(ClaimTypes.Upn)!, text));
        return RedirectToPage(new { ProposalId });
    }
}