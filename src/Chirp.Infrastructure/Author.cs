namespace Chirp.Infrastructure;
public class Author
{
    public int AuthorId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public ICollection<Cheep> Cheeps { get; set; } = new List<Cheep>();
    public ICollection<Author> Followers { get; set; } = new List<Author>();
    public ICollection<Author> Following { get; set; } = new List<Author>();
}