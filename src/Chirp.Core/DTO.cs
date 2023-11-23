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

    public override bool Equals(object? obj)
    {
        return this.Author == ((CheepDTO)obj!).Author && this.Text == ((CheepDTO)obj).Text && this.TimeStamp == ((CheepDTO)obj).TimeStamp;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}