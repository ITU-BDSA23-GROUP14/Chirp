using System.Net.Mail;
using Chirp.Core;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Chirp.Infrastructure;

public class CheepRepository : ICheepRepository
{
    private readonly ChirpDBContext _dbContext;
    private readonly IValidator<CheepCreateDTO> _validator;

    public CheepRepository(ChirpDBContext dbContext, IValidator<CheepCreateDTO> validator)
    {
        _dbContext = dbContext;
        _validator = validator;
    }

    public async Task CreateCheep(CheepCreateDTO cheep)
    {
        var validationResult = await _validator.ValidateAsync(cheep);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }


        var user = await _dbContext.Authors.SingleOrDefaultAsync(u => u.Name == cheep.Author);

        // Check if author exists. If not, create the author
        if (user == null)
        {
            Author author = new() { Name = cheep.Author, Email = cheep.Email };

            await _dbContext.Authors.AddAsync(author);
            await _dbContext.SaveChangesAsync();
            user = author;
        }

        var entity = new Cheep
        {
            Author = user,
            Text = cheep.Text,
            TimeStamp = DateTime.UtcNow
        };

        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public List<CheepDTO> GetCheepDTOs(int page)
    {
        if (page > 0)
        {
            page -= 1;
        }
        return (from c in _dbContext.Cheeps
                orderby c.TimeStamp descending
                select new CheepDTO
                {
                    Author = c.Author.Name,
                    Text = c.Text ?? "",
                    TimeStamp = c.TimeStamp.ToString("dd/MM/yy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)
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
                orderby c.TimeStamp descending
                select new CheepDTO
                {
                    Author = c.Author.Name,
                    Text = c.Text ?? "",
                    TimeStamp = c.TimeStamp.ToString("dd/MM/yy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture)
                }).Skip(32 * page).Take(32).ToList();
    }

    /*
    public List<CheepDTO> GetCheepDTOsFromFollowing(string authorName, int page)
    {
        if (page > 0)
        {
            page -= 1;
        }

        // Find the authors following list
        var followingAuthorIds =   (from a in _dbContext.Authors
                                    join f in _dbContext.Followings on a.AuthorId equals f.AuthorId
                                    where a.Name == authorName
                                    select f.FollowedAuthorId).ToList();


        // Query all cheeps by followed users and return the 32 cheeps on the current page
        
    }
    */

    /*
    public List<CheepDTO> GetCheepDTOsForPrivateTimeline(string authorName, int page)
    {
        if (page > 0)
        {
            page -= 1;
        }

        var ownCheeps = GetCheepDTOsFromAuthor(string authorName, int page);
        var followerCheeps = GetCheepDTOsFromFollowing(string authorName, int page);

        var cheeps = 
         
    }
    */

}