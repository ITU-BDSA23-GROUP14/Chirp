public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps();
    public List<CheepViewModel> GetCheeps(int pageNum);
    public List<CheepViewModel> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    // These would normally be loaded from a database for example
    private static readonly List<CheepViewModel> _cheeps = new()
        {
            new CheepViewModel("Helge", "Hello, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "Hej, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "Hello, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Rasmus", "Hej, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Helge", "Hello, BDSA students!", UnixTimeStampToDateTimeString(1690892208)),
            new CheepViewModel("Jonas", "Hej, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Niklas", "Hej, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
            new CheepViewModel("Ida", "Hej, velkommen til kurset.", UnixTimeStampToDateTimeString(1690895308)),
        };

    public List<CheepViewModel> GetCheeps()
    {
        return _cheeps;
    }

    public List<CheepViewModel> GetCheeps(int pageNum)
    {
        if (pageNum == 0)
        {
            pageNum = 1;
        }
        int startingCheep = (pageNum - 1) * 5;

        if (_cheeps.Count >= startingCheep + 5)
        {
            return _cheeps.GetRange(startingCheep, 5);
        }
        else if (_cheeps.Count < startingCheep)
        {
            return _cheeps.GetRange(0, 5);
        }
        else
        {
            return _cheeps.GetRange(startingCheep, _cheeps.Count - startingCheep);
        }
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        // filter by the provided author name
        return _cheeps.Where(x => x.Author == author).ToList();
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
