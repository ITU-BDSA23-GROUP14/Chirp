using ViewModel;

public interface ICheepService
{
    //public List<CheepViewModel> GetCheeps();
    public List<CheepViewModel> GetSelectCheeps(int pageNum);
    public List<CheepViewModel> GetCheepsFromAuthor(string author, int pageNum);
}

public class CheepService : ICheepService
{
    private readonly DBFacade facade;

    public CheepService()
    {
        this.facade = new DBFacade();
    }

    public List<CheepViewModel> GetSelectCheeps(int pageNum)
    {
        var _cheeps = facade.GetCheeps(pageNum);
        return _cheeps;
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author, int pageNum)
    {
        // filter by the provided author name
        var _cheeps = facade.GetCheepsFromAuthor(author, pageNum);
        return _cheeps;
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
