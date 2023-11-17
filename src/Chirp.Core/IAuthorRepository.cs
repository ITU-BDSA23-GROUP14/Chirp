namespace Chirp.Core;

public interface IAuthorRepository
{
    public AuthorDTO? GetAuthorByName(string name);
    public AuthorDTO? GetAuthorByEmail(string email);
    public void CreateAuthor(string name, string email);
    public Task AddFollowing(string user, string target);
    public Task RemoveFollowing(string user, string target);
    public bool IsAuthorFollowingAuthor(string user, string target);
}