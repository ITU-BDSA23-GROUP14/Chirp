using Microsoft.EntityFrameworkCore;
using ViewModel;
using Chirp.Models;

public interface ICheepRepository
{
    //public List<CheepViewModel> GetCheeps();
    public List<CheepViewModel> GetCheeps(int page);
    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page);
}

public class CheepRepository : ICheepRepository
{
    private readonly ChirpDBContext _dbContext;

    public CheepRepository(ChirpDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<CheepViewModel> GetCheeps(int page)
    {
        if (page > 0)
        {
            page -= 1;
        }
        return (from c in _dbContext.Cheeps
                orderby c.TimeStamp
                select new CheepViewModel(c.Author.Name, c.Text ?? "", c.TimeStamp.ToString("dd/MM/yy HH:mm:ss"))).Skip(32 * page).Take(32).ToList();
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author, int page)
    {
        if (page > 0)
        {
            page -= 1;
        }
        return (from c in _dbContext.Cheeps
                where c.Author.Name == author
                orderby c.TimeStamp
                select new CheepViewModel(author, c.Text ?? "", c.TimeStamp.ToString("dd/MM/yy HH:mm:ss"))).Skip(32 * page).Take(32).ToList();
    }
}