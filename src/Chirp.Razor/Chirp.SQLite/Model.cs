namespace Chirp.Models;

public class Author
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public ICollection<Cheep> Cheeps { get; set; } = new List<Cheep>();
}

public class Cheep
{
    public int CheepId { get; set; }
    public required Author Author { get; set; }
    public string? Text { get; set; }
    public DateTime TimeStamp { get; set; }
}