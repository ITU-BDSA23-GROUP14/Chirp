namespace Chirp.Infrastructure;

public class Cheep
{
    public int CheepId { get; set; }
    public int AuthorId { get; set; }
    public required Author Author { get; set; }
    public string? Text { get; set; }
    public DateTime TimeStamp { get; set; }
}