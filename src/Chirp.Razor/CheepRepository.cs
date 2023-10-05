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
        TestAdd();
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

    public void TestAdd() {
        var author = new Author { Email = "jfho@itu.dk", Name = "jonas", Cheeps = new List<Cheep>(), Id = 1};
        var cheep1 = new Cheep { Author = author, CheepId = 0, Text = "hello", TimeStamp = DateTime.UtcNow};
        _dbContext.Add(author);
        _dbContext.Add(cheep1);
        _dbContext.SaveChanges();
    }
}