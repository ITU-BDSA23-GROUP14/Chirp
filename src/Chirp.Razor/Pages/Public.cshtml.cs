using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ViewModel;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepRepository _repository;
    public List<CheepViewModel> Cheeps { get; set; }

    public PublicModel(ICheepRepository repository)
    {
        Cheeps = new();
        _repository = repository;
    }

    public ActionResult OnGet()
    {
        int.TryParse(Request.Query["page"], out int page);
        Console.WriteLine($"page is {page}");
        Cheeps = _repository.GetCheeps(page);
        return Page();
    }
}
