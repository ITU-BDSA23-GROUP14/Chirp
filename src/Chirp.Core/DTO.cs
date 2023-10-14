namespace Chirp.Core;

public class AuthorDTO
{
    public required string Name { get; set; }
}

public class CheepDTO
{
    public required string Author { get; set; }
    public required string Text { get; set; }
    public required string TimeStamp { get; set; }
}