using Microsoft.EntityFrameworkCore;
using Chirp.Models;
using Chirp.DTO;

public interface ICheepRepository
{
    public List<CheepDTO> GetCheepDTOs(int page);
    public List<CheepDTO> GetCheepDTOsFromAuthor(string author, int page);
}

public class CheepRepository : ICheepRepository
{
    private readonly ChirpDBContext _dbContext;

    public CheepRepository(ChirpDBContext dbContext)
    {
        _dbContext = dbContext;
        DbInitializer.SeedDatabase(_dbContext);
    }

    public List<CheepDTO> GetCheepDTOs(int page)
    {
        if (page > 0)
        {
            page -= 1;
        }
        return (from c in _dbContext.Cheeps
                orderby c.TimeStamp
                select new CheepDTO
                {
                    Author = c.Author.Name,
                    Text = c.Text ?? "",
                    TimeStamp = c.TimeStamp.ToString("dd/MM/yy HH:mm:ss")
                }).Skip(32 * page).Take(32).ToList();
    }

    public List<CheepDTO> GetCheepDTOsFromAuthor(string author, int page)
    {
        if (page > 0)
        {
            page -= 1;
        }
        return (from c in _dbContext.Cheeps
                where c.Author.Name == author
                orderby c.TimeStamp
                select new CheepDTO
                {
                    Author = c.Author.Name,
                    Text = c.Text ?? "",
                    TimeStamp = c.TimeStamp.ToString("dd/MM/yy HH:mm:ss")
                }).Skip(32 * page).Take(32).ToList();
    }
}