using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;

namespace Chirp.Web;

public class AboutmeModel : PageModel
{
    private readonly ICheepRepository _cheepRepository;
    private readonly IAuthorRepository _authorRepository;
    public List<CheepDTO> Cheeps { get; set; }
    public List<string> FollowedAuthors { get; set; }

    public AboutmeModel(ICheepRepository cheepRepository, IAuthorRepository authorRepository)
    {
        Cheeps = new();
        FollowedAuthors = new();
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
    }

    public ActionResult OnGet()
    {
        string name = User.Identity!.Name!;
        if (_authorRepository.GetAuthorByName(name) == null) {
            string email = User.Claims.FirstOrDefault(c => c.Type == "emails")!.Value;
            _authorRepository.CreateAuthor(name, email);    
        }
        Cheeps = _cheepRepository.GetAllCheepDTOsFromAuthor(name);
        FollowedAuthors = _authorRepository.GetFollowedAuthors(name);

        return Page();
    }
}