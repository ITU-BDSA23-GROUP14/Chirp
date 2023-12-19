using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Chirp.Core;

namespace Chirp.Web;

public class UserTimelineModel : PageModel
{
    private readonly ICheepRepository _cheepRepository;
    private readonly IAuthorRepository _authorRepository;
    public List<CheepDTO> Cheeps { get; set; }
    public List<string> FollowedAuthors { get; set; }
    private readonly HttpClient client;
    public bool HasNextPage { get; set; }
    public int CurrentPage { get; set; }

    public UserTimelineModel(ICheepRepository cheepRepository, IAuthorRepository authorRepository)
    {
        Cheeps = new();
        FollowedAuthors = new();
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
        client = new HttpClient();
    }

    public async Task<IActionResult> OnGet(string author)
    {
        string name = User.Identity!.Name!;
        if (_authorRepository.GetAuthorByName(name) == null)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == "emails")!.Value;
            _authorRepository.CreateAuthor(name, email);
        }
        if (User.Identity!.Name == author)
        {
            int.TryParse(Request.Query["page"], out int page);
            if (page == 0)
            {
                CurrentPage = 1;
            }
            else
            {
                CurrentPage = page;
            }
            Cheeps = await _cheepRepository.GetCheepDTOsForPrivateTimeline(author, page);
            var cheepsOnNextPage = await _cheepRepository.GetCheepDTOsForPrivateTimeline(author, CurrentPage + 1);
            if (cheepsOnNextPage.Count() > 0)
            {
                HasNextPage = true;
            }
            else
            {
                HasNextPage = false;
            }

            if (User.Identity!.IsAuthenticated)
            {
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

        else
        {
            int.TryParse(Request.Query["page"], out int page);
            if (page == 0)
            {
                CurrentPage = 1;
            }
            else
            {
                CurrentPage = page;
            }
            Cheeps = _cheepRepository.GetCheepDTOsFromAuthor(author, page);
            if (_cheepRepository.GetCheepDTOsFromAuthor(author, CurrentPage + 1).Count() > 0)
            {
                HasNextPage = true;
            }
            else
            {
                HasNextPage = false;
            }

            if (User.Identity!.IsAuthenticated)
            {
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

    public async Task<IActionResult> OnPost(CheepCreateDTO newCheep)
    {
        String email = User.Claims.FirstOrDefault(c => c.Type == "emails")!.Value;

        var cheep = new CheepCreateDTO { Text = newCheep.Text, Author = User.Identity!.Name!, Email = email };

        await _cheepRepository.CreateCheep(cheep);

        return Redirect($"/{User.Identity!.Name}");
    }

    public async Task<IActionResult> OnPostFollow(string authorToFollow)
    {
        string currentUser = User.Identity!.Name!;
        await _authorRepository.AddFollowing(currentUser, authorToFollow);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostUnfollow(string authorToUnfollow)
    {
        string currentUser = User.Identity!.Name!;
        await _authorRepository.RemoveFollowing(currentUser, authorToUnfollow);
        return RedirectToPage();
    }

    public async Task<string> GetGithubPictureURL(string author)
    {
        if (string.IsNullOrWhiteSpace(author))
        {
            return "/images/icon1.png"; // Default image for null or empty author
        }

        var githubUserUrl = $"https://github.com/{author}.png";

        try
        {
            var response = await client.GetAsync(githubUserUrl);
            var content = await response.Content.ReadAsStringAsync();

            if (content.Contains("Not Found")) {
                return "/images/icon1.png";
            }
            else
            {
                return $"https://github.com/{author}.png";
            }
        }
        catch (Exception)
        {
            // Log the exception (ex.Message) or handle it as needed
            return "/images/icon1.png"; // Fallback image in case of an exception
        }
    }

}