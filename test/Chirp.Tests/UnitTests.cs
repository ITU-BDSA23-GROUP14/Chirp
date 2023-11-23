using System.Runtime.InteropServices;
using Chirp.Core;
using Chirp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel.Resolution;

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
        // Arrange
        _AuthorRepository.CreateAuthor("Batman", "bruce@wayneenterprises.dc");

        // Act
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
        var lst = _CheepRepository.GetCheepDTOs(0);

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
    public async Task GetCheepDTOsForPrivateTimeline_returns_newest_32_cheeps_from_following_authors()
    {
        // Arrange
        _context.Authors.Add(new Author { Name = "Batman2", Email = "bruce2@wayneenterprises.dc" });
        await _context.SaveChangesAsync();

        // Act
        await _AuthorRepository.AddFollowing("Batman2", "Jacqualine Gilcoine");
        await _AuthorRepository.AddFollowing("Batman2", "Johnnie Calixto");

        List<CheepDTO> actualDTOs = new();
        actualDTOs.AddRange(_CheepRepository.GetCheepDTOsFromAuthor("Jacqualine Gilcoine", 1));
        actualDTOs.AddRange(_CheepRepository.GetCheepDTOsFromAuthor("Johnnie Calixto", 1));

        actualDTOs = (from a in actualDTOs orderby a.TimeStamp descending select a).Take(32).ToList();

        List<CheepDTO> testingDTOs = await _CheepRepository.GetCheepDTOsForPrivateTimeline("Batman2", 1);

        // Assert
        Assert.Equal(testingDTOs, actualDTOs);
    }

    [Fact]
    public async Task AddFollowing_adds_follow_to_database()
    {
        // Arrange
        _AuthorRepository.CreateAuthor("Bamse", "bamse@DR.dk");
        _AuthorRepository.CreateAuthor("Kylling", "kylling@DR.dk");

        // Act
        await _AuthorRepository.AddFollowing("Bamse", "Kylling");

        // Assert
        Assert.True(_AuthorRepository.IsAuthorFollowingAuthor("Bamse", "Kylling"));
    }

    [Fact]
    public async Task RemoveFollowing_removes_follow_from_database()
    {
        // Arrange
        _AuthorRepository.CreateAuthor("Batman3", "bruce3@wayneenterprises.dc");
        _AuthorRepository.CreateAuthor("Flash", "henry@allen.dc");

        //Act
        await _AuthorRepository.AddFollowing("Flash", "Batman3");
        await _AuthorRepository.RemoveFollowing("Flash", "Batman3");

        //Assert
        Assert.False(_AuthorRepository.IsAuthorFollowingAuthor("Flash", "Batman3"));
    }

    [Fact]
    public void IsAuthorFollowingAuthorFalse()
    {
        // Arrange
        _AuthorRepository.CreateAuthor("test", "test@test.com");
        _AuthorRepository.CreateAuthor("test2", "test2@test.com");

        // Assert
        var isFollowing = _AuthorRepository.IsAuthorFollowingAuthor("test", "test2");
        Assert.False(isFollowing);
    }
}