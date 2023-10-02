using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ViewModel;

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

    public ActionResult OnGet(int pageNum)
    {
        Console.WriteLine($"page is {pageNum}");
        Cheeps = _service.GetSelectCheeps(pageNum);
        return Page();
    }
}
