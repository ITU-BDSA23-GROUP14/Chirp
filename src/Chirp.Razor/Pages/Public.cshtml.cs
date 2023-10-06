using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ViewModel;
using Service;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepViewModel> Cheeps { get; set; }

    public PublicModel(ICheepService service)
    {
        Cheeps = new();
        _service = service;
    }

    public ActionResult OnGet()
    {
        int.TryParse(Request.Query["page"], out int page);
        Console.WriteLine($"page is {page}");
        Cheeps = _service.GetSelectCheeps(page);
        return Page();
    }
}