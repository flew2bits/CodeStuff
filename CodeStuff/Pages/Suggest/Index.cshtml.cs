using Baseline;
using CodeStuff.TalkSuggestion;
using CodeStuff.TalkSuggestion.Commands;
using CodeStuff.TalkSuggestion.Views;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeStuff.Pages.Suggest;

[Authorize]
public class Index : PageModel
{
    public ActiveSuggestion[] Suggestions { get; set; } = Array.Empty<ActiveSuggestion>();

    [BindProperty] public TopicSuggestion NewSuggestion { get; set; } = new("", "");

    public async Task OnGet([FromServices] GetAll<ActiveSuggestion> getAllSuggestions)
    {
        Suggestions = (await getAllSuggestions()).ToArray();
    }

    public async Task<IActionResult> OnPost(
        [FromServices] SuggestionCommandHandler commandHandler,
        [FromServices] IValidator<TopicSuggestion> validator
    )
    {
        var result = await validator.ValidateAsync(NewSuggestion);
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return RedirectToPage();
        }

        await commandHandler.HandleCommand(
            Guid.NewGuid(),
            new SubmitSuggestion(NewSuggestion.Topic, NewSuggestion.AdditionalDetails,
                User.FullName() ?? throw new InvalidOperationException("User has no name")));

        return RedirectToPage();
    }

    public record TopicSuggestion(string Topic, string AdditionalDetails);

    public class TopicSuggestionValidator : AbstractValidator<TopicSuggestion>
    {
        public TopicSuggestionValidator()
        {
            RuleFor(s => s.Topic).NotEmpty();
        }
    }
}