using Microsoft.EntityFrameworkCore;

namespace Chirp.Models;

public class ChirpDBContext : DbContext
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Cheep> Cheeps { get; set; }

    public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Author>()
            .HasMany(e => e.Cheeps)
            .WithOne(e => e.Author)
            .HasForeignKey(e => e.AuthorId)
            .IsRequired();
    }
}

public class Author
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public ICollection<Cheep> Cheeps { get; set; } = new List<Cheep>();
}

public class Cheep
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public required Author Author { get; set; }
    public string? Text { get; set; }
    public DateTime TimeStamp { get; set; }
}