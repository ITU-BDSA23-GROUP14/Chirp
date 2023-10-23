namespace Chirp.Core;

public interface ICheepRepository
{
    public List<CheepDTO> GetCheepDTOs(int page);
    public List<CheepDTO> GetCheepDTOsFromAuthor(string author, int page);
    public AuthorDTO? GetAuthorByName(string name);
    public AuthorDTO? GetAuthorByEmail(string email);
    public void CreateAuthor(string name, string email);
}