using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Tests;

public class UnitTests : IDisposable
{
    private readonly ChirpDBContext _context;
    private readonly CheepRepository _CheepRepository;
    private readonly AuthorRepository _AuthorRepository;
    private readonly CheepCreateValidator _CheepValidator;

    public UnitTests()
    {
        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseInMemoryDatabase(databaseName: "DBMemoryUnit");
        _context = new ChirpDBContext(builder.Options);

        _context.Database.EnsureCreated();

        DbInitializer.SeedDatabase(_context);

        _CheepValidator = new CheepCreateValidator();
        _CheepRepository = new CheepRepository(_context, _CheepValidator);
        _AuthorRepository = new AuthorRepository(_context);
    }

    public void Dispose()
    {
        try
        {
            _context.Dispose();
        }
        finally
        {
        }

        GC.SuppressFinalize(this);
    }

    [Fact]
    public void CreateAuthor_Adds_Author_To_Database()
    {
        // Act
        _AuthorRepository.CreateAuthor("Batman", "bruce@wayneenterprises.dc");
        var created = _context.Authors.SingleOrDefault(c => c.Name == "Batman");

        // Assert
        Assert.NotNull(created);
    }

    [Fact]
    public async Task CreateCheep_Adds_Cheep_To_DatabaseAsync()
    {
        // Add to database
        await _CheepRepository.CreateCheep(
            new CheepCreateDTO
            {
                Text = "Hello world",
                Author = "Superman",
                Email = "clark@notkent.dc"
            });

        var cheeps = _CheepRepository.GetCheepDTOsFromAuthor("Superman", 1);

        // Assert
        Assert.Contains(cheeps, cheep => cheep.Text == "Hello world");
    }

    [Fact]
    public void GetAuthorByMail_returns_proper_name()
    {
        // Act
        var author = _AuthorRepository.GetAuthorByEmail("ropf@itu.dk");

        // Assert
        Assert.Equal("Helge", author?.Name);
    }

    [Fact]
    public void GetAuthorByName_returns_proper_name()
    {
        //Act
        var author = _AuthorRepository.GetAuthorByName("Helge");

        // Assert
        Assert.Equal("Helge", author?.Name);
    }

    [Fact]
    public void GetCheepDTOs_returns_32_cheeps()
    {
        // Act
        var lst = _CheepRepository.GetCheepDTOsForPublicTimeline(0);

        // Assert
        Assert.Equal(32, lst.Count);
    }

    [Fact]
    public void GetCheepDTOsFromAuthor_returns_proper_cheep_format()
    {
        // Act
        var lst = _CheepRepository.GetCheepDTOsFromAuthor("Helge", 0);
        var cheep = lst[0];

        // Assert
        Assert.Equal("Helge", cheep.Author);
        Assert.Equal("Hello, BDSA students!", cheep.Text);
        Assert.Equal("01/08/23 12:16:48", cheep.TimeStamp);
    }

    [Theory]
    [InlineData("Helge", 1, "Hello, BDSA students!")]
    [InlineData("Rasmus", 1, "Hej, velkommen til kurset.")]
    public void GetCheepDTOsFromAuthor_returns_cheeps_from_author(string author, int page, string expectedMessage)
    {
        // Act
        var cheeps = _CheepRepository.GetCheepDTOsFromAuthor(author, page);

        // Assert
        Assert.Contains(cheeps, cheep => cheep.Text == expectedMessage);
    }

    [Fact]
    public void GetCheepDTOsFromAuthor_returns_newest_32_cheeps_from_author()
    {
        // Act
        var cheeps = _CheepRepository.GetCheepDTOsFromAuthor("Jacqualine Gilcoine", 1);

        // Assert
        Assert.True(cheeps.Count == 32);
        Assert.True(DateTime.Parse(cheeps[0].TimeStamp) > DateTime.Parse(cheeps[1].TimeStamp));
    }

    [Fact]
    public async Task AddFollowing_yourself_fails()
    {
        // Arrange
        _AuthorRepository.CreateAuthor("yourself", "you@you.com");
        
        //Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => 
        {
            await _AuthorRepository.AddFollowing("yourself", "yourself");
        });
        
        Assert.False(_AuthorRepository.IsAuthorFollowingAuthor("yourself", "yourself"));
    }

    [Fact]
    public async Task RemoveFollowing_on_unfollowed_nonexistent_target_fails()
    {
        // Arrange
        _AuthorRepository.CreateAuthor("Hello","world@something.com");
        
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => 
            { 
                await _AuthorRepository.RemoveFollowing("Hello", "World");
            });
    }
    
    [Fact]
    public async Task RemoveFollowing_nonexistent_user_on_unfollowed_target_fails()
    {   
        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () => 
            {
                await _AuthorRepository.RemoveFollowing("World", "Hello");
            });
    }

    [Fact]
    public async Task CreateCheep_too_long_text_fails()
    {
        // Arrange
        var cheep = new CheepCreateDTO 
        {
            // Text length is over 160 character limit
            Text = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
            Author = "Superman",
            Email = "clark@notkent.dc"
        };
        
        // Act & Assert
        await Assert.ThrowsAsync<FluentValidation.ValidationException>(async () => 
            {
                await _CheepRepository.CreateCheep(cheep);
            });
    }

    [Fact]
    public void GetAuthorByEmail_with_nonexistent_user_returns_null()
    {
        // Act
        var nonexistantUser = _AuthorRepository.GetAuthorByEmail("foo@bar.com");

        // Assert
        Assert.Null(nonexistantUser);
    }
}