using System.Security.Claims;
using CodeStuff.EntityShared.Commands;
using CodeStuff.TalkSuggestion;
using CodeStuff.TalkSuggestion.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeStuff.Pages.Suggest;

public class Details : PageModel
{
    private readonly SuggestionCommandHandler _commandHandler;
    [BindProperty(SupportsGet = true)] public Guid SuggestionId { get; set; }

    public Details(SuggestionCommandHandler commandHandler)
    {
        _commandHandler = commandHandler;
    }

    public SuggestionDetail Detail { get; set; } = null!;

    public async Task<IActionResult> OnGet([FromServices] Find<Guid, SuggestionDetail?> findSuggestionDetail)
    {
        var detail = await findSuggestionDetail(SuggestionId);
        if (detail is null) return NotFound();
        
        Detail = detail;

        return Page();
    }

    public async Task<IActionResult> OnPostAddComment(string text, Guid? inReplyTo)
    {
        await _commandHandler.HandleCommand(SuggestionId, inReplyTo.HasValue
            ? new ReplyToComment(User.FindFirstValue(ClaimTypes.Upn)!, text, inReplyTo.Value)
            : new StartCommentThread(User.FindFirstValue(ClaimTypes.Upn)!, text));
        return RedirectToPage();
    }
}