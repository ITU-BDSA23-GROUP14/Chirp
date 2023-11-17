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

    public async Task AddFollowing(string user, string target)
    {
        var author = _dbContext.Authors.FirstOrDefault(author => author.Name == user);
        var authorToFollow = _dbContext.Authors.FirstOrDefault(author => author.Name == target);

        if (author == null)
        {
            throw new InvalidOperationException($"The user {user} does not exist.");
        }
        else if (authorToFollow == null)
        {
            throw new InvalidOperationException($"The user {target} does not exist.");
        }
        else
        {
            author.Following.Add(authorToFollow);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task RemoveFollowing(string user, string target)
    {
        var author = _dbContext.Authors.FirstOrDefault(author => author.Name == user);
        var authorToUnfollow = _dbContext.Authors.FirstOrDefault(author => author.Name == target);

        if (author == null)
        {
            throw new InvalidOperationException($"The user {user} does not exist.");
        }
        else if (authorToUnfollow == null)
        {
            throw new InvalidOperationException($"The user {target} does not exist.");
        }
        else
        {
            author.Following.Remove(authorToUnfollow);
            await _dbContext.SaveChangesAsync();
        }
    }

    public bool IsAuthorFollowingAuthor(string user, string target)
    {
        var author = _dbContext.Authors.FirstOrDefault(author => author.Name == user);
        var authorTarget = _dbContext.Authors.FirstOrDefault(author => author.Name == target);

        if (author == null)
        {
            throw new InvalidOperationException($"The user {user} does not exist.");
        }
        else if (authorTarget == null)
        {
            throw new InvalidOperationException($"The user {target} does not exist.");
        }
        else
        {
            return author.Following.Contains(authorTarget);
        }
    }
}