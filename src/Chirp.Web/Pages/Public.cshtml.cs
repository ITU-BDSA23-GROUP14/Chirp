using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Chirp.Web;

public class PublicModel : PageModel
{
    private readonly ICheepRepository _repository;
    public List<CheepDTO> Cheeps { get; set; }

    public PublicModel(ICheepRepository repository)
    {
        Cheeps = new();
        _repository = repository;
    }

    public async Task<IActionResult> OnPost(CheepCreateDTO newCheep)
    {
        String email = User.Claims.FirstOrDefault(c => c.Type == "emails")!.Value;

        var cheep = new CheepCreateDTO { Text = newCheep.Text, Author = User.Identity!.Name!, Email = email };

        await _repository.CreateCheep(cheep);

        return Redirect($"/{User.Identity!.Name}");
    }

    public ActionResult OnGet()
    {
        int.TryParse(Request.Query["page"], out int page);
        Cheeps = _repository.GetCheepDTOs(page);
        return Page();
    }
}