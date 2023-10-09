using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ViewModel;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepRepository _repository;
    public List<CheepViewModel> Cheeps { get; set; }

    public UserTimelineModel(ICheepRepository repository)
    {
        Cheeps = new();
        _repository = repository;
    }

    public ActionResult OnGet(string author)
    {
        int.TryParse(Request.Query["page"], out int page);
        Console.WriteLine($"page is {page}");
        Cheeps = _repository.GetCheepsFromAuthor(author, page);
        return Page();
    }
}
