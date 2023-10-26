namespace Chirp.Core;

public interface ICheepRepository
{
    public void CreateCheep(string cheepText, string authorName, string authorEmail);
    public List<CheepDTO> GetCheepDTOs(int page);
    public List<CheepDTO> GetCheepDTOsFromAuthor(string author, int page);
}