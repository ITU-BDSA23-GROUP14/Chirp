using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;

namespace Chirp.Web;

public class PublicModel : PageModel
{
    private readonly ICheepRepository _cheepRepository;
    private readonly IAuthorRepository _authorRepository;
    public List<CheepDTO> Cheeps { get; set; }
    public List<string> FollowedAuthors { get; set; }

    public PublicModel(ICheepRepository cheepRepository, IAuthorRepository authorRepository)
    {
        Cheeps = new();
        FollowedAuthors = new();
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
    }

    public async Task<IActionResult> OnPost(CheepCreateDTO newCheep)
    {
        String email = User.Claims.FirstOrDefault(c => c.Type == "emails")!.Value;

        var cheep = new CheepCreateDTO { Text = newCheep.Text, Author = User.Identity!.Name!, Email = email };

        await _cheepRepository.CreateCheep(cheep);

        return Redirect($"/{User.Identity!.Name}");
    }

    public ActionResult OnGet()
    {
        int.TryParse(Request.Query["page"], out int page);
        Cheeps = _cheepRepository.GetCheepDTOs(page);

        // List of the cheep authors that the user follows
        if (User.Identity!.IsAuthenticated){
            foreach (var c in Cheeps)
            {
                if (_authorRepository.IsAuthorFollowingAuthor(User.Identity!.Name!, c.Author)) 
                {
                    FollowedAuthors.Add(c.Author);    
                }
            }
        }
        
         return Page();
    } 
}