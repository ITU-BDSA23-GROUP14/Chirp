using Chirp.Core;

namespace Chirp.Infrastructure;

public class CheepRepository : ICheepRepository
{
    private readonly ChirpDBContext _dbContext;

    public CheepRepository(ChirpDBContext dbContext)
    {
        _dbContext = dbContext;
        DbInitializer.SeedDatabase(_dbContext);
    }

    public void CreateAuthor(string name, string email)
    {
        if (_dbContext.Authors.Any(a => a.Email == email))
        {
            throw new InvalidOperationException($"The email address {email} is already in use.");
        }

        if (_dbContext.Authors.Any(a => a.Name == name))
        {
            throw new InvalidOperationException($"The name {name} is already taken.");
        }

        var author = new Author { Name = name, Email = email };

        _dbContext.Authors.Add(author);
        _dbContext.SaveChanges();
    }

    public void CreateCheep(string cheepText, string authorName, string authorEmail)
    {
        // Check if author exists. If not, create the author
        var author = _dbContext.Authors.FirstOrDefault(author => author.Name == authorName);
        if (author == null)
        {
            author = new Author { Name = authorName, Email = authorEmail };
            _dbContext.Add(author);
            _dbContext.SaveChanges();
        }

        var cheep = new Cheep { Author = author, Text = cheepText };

        _dbContext.Add(cheep);
        _dbContext.SaveChanges();
    }

    public AuthorDTO? GetAuthorByEmail(string email)
    {
        var author = _dbContext.Authors.FirstOrDefault(author => author.Email == email);
        if (author == null)
        {
            return null;
        }

        return new AuthorDTO { Name = author.Name };
    }

    public AuthorDTO? GetAuthorByName(string name)
    {
        var author = _dbContext.Authors.FirstOrDefault(author => author.Name == name);
        if (author == null)
        {
            return null;
        }

        return new AuthorDTO { Name = author.Name };
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