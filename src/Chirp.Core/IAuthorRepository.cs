namespace Chirp.Core;

public interface IAuthorRepository
{
    public AuthorDTO? GetAuthorByName(string name);
    public AuthorDTO? GetAuthorByEmail(string email);
    public void CreateAuthor(string name, string email);
}