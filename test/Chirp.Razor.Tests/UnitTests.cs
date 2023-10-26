using Chirp.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Tests;

public class UnitTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly ChirpDBContext _context;
    private readonly CheepRepository _CheepRepository;
    private readonly AuthorRepository _AuthorRepository;

    public UnitTests()
    {
        // Arrange
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(_connection);
        _context = new ChirpDBContext(builder.Options);

        _context.Database.EnsureCreated();

        DbInitializer.SeedDatabase(_context); 

        _CheepRepository = new CheepRepository(_context);
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
            _connection.Dispose();
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
    public void CreateCheep_Adds_Cheep_To_Database()
    {
        // Add to database
        _CheepRepository.CreateCheep("Hello world", "Superman", "clark@notkent.dc");
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
    public void GetCheepDTOsFromAuthor_returns_first_32_cheeps_from_author()
    {
        // Act
        var cheeps = _CheepRepository.GetCheepDTOsFromAuthor("Jacqualine Gilcoine", 1);

        // Assert
        Assert.True(cheeps.Count == 32);
        Assert.True(DateTime.Parse(cheeps[0].TimeStamp) < DateTime.Parse(cheeps[1].TimeStamp));
    }
}