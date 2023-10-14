namespace Chirp.Core;

public interface ICheepRepository
{
    public List<CheepDTO> GetCheepDTOs(int page);
    public List<CheepDTO> GetCheepDTOsFromAuthor(string author, int page);
}