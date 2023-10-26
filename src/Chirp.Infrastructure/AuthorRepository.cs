using Chirp.Core;

namespace Chirp.Infrastructure;

public class AuthorRepository : IAuthorRepository
{
    private readonly ChirpDBContext _dbContext;

    public AuthorRepository(ChirpDBContext dbContext)
    {
        _dbContext = dbContext;
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
}