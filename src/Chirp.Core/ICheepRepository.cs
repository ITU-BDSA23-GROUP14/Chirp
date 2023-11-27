namespace Chirp.Core;

public interface ICheepRepository
{
    public Task CreateCheep(CheepCreateDTO cheepCreate);
    public List<CheepDTO> GetCheepDTOsForPublicTimeline(int page);
    public List<CheepDTO> GetCheepDTOsFromAuthor(string author, int page);
    public Task<List<CheepDTO>> GetCheepDTOsForPrivateTimeline(string author, int page);
    public List<CheepDTO> GetAllCheepDTOsFromAuthor(string author);
}