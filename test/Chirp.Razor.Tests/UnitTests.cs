using Chirp.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace UnitTests;

public class UnitTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly ChirpDBContext _context;
    private readonly CheepRepository _repository;

    public UnitTests()
    {
        // Arrange
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        var builder = new DbContextOptionsBuilder<ChirpDBContext>().UseSqlite(_connection);
        _context = new ChirpDBContext(builder.Options);

        _context.Database.EnsureCreated();

        DbInitializer.SeedDatabase(_context); 

        _repository = new CheepRepository(_context);
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
        _repository.CreateAuthor("Batman", "bruce@wayneenterprises.dc");
        var created = _context.Authors.SingleOrDefault(c => c.Name == "Batman");
        
        // Assert
        Assert.NotNull(created);
    }

    [Fact]
    public void CreateCheep_Adds_Cheep_To_Database()
    {
        // Add to database
        _repository.CreateCheep("Hello world", "Superman", "clark@notkent.dc");
        var cheeps = _repository.GetCheepDTOsFromAuthor("Superman", 1);

        // Assert
        Assert.Contains(cheeps, cheep => cheep.Text == "Hello world");
    }

    [Fact]
    public void GetAuthorByMail_returns_proper_name()
    {
        // Act
        var author = _repository.GetAuthorByEmail("ropf@itu.dk");
        
        // Assert
        Assert.Equal("Helge", author?.Name);
    }

    [Fact]
    public void GetAuthorByName_returns_proper_name()
    {
        //Act
        var author = _repository.GetAuthorByName("Helge");
        
        // Assert
        Assert.Equal("Helge", author?.Name);
    }

    [Fact]
    public void GetCheepDTOs_returns_32_cheeps()
    {
        // Act
        var lst = _repository.GetCheepDTOs(0);
        
        // Assert
        Assert.Equal(32, lst.Count);
    }

    [Fact]
    public void GetCheepDTOsFromAuthor_returns_proper_cheep_format()
    {
        // Act
        var lst = _repository.GetCheepDTOsFromAuthor("Helge", 0);
        var cheep = lst[0];
        
        // Assert
        Assert.Equal("Helge", cheep.Author);
        Assert.Equal("Hello, BDSA students!", cheep.Text);
        Assert.Equal("01-08-23 12:16:48", cheep.TimeStamp);
    }

    [Theory]
    [InlineData("Helge", 1, "Hello, BDSA students!")]
    [InlineData("Rasmus", 1, "Hej, velkommen til kurset.")]
    public void GetCheepDTOsFromAuthor_returns_cheeps_from_author(string author, int page, string expectedMessage)
    {
        // Act
        var cheeps = _repository.GetCheepDTOsFromAuthor(author, page);
        
        // Assert
        Assert.Contains(cheeps, cheep => cheep.Text == expectedMessage);
    }

    [Fact]
    public void GetCheepDTOsFromAuthor_returns_first_32_cheeps_from_author()
    {
        // Act
        var cheeps = _repository.GetCheepDTOsFromAuthor("Jacqualine Gilcoine", 1);

        // Assert
        Assert.True(cheeps.Count == 32);
        Assert.True(DateTime.Parse(cheeps[0].TimeStamp) < DateTime.Parse(cheeps[1].TimeStamp));
    }
}