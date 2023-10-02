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
    /*     // These would normally be loaded from a database for example
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
            }; */

    /* public List<CheepViewModel> GetCheeps()
    {
        var _cheeps = facade.GetCheeps();
        return _cheeps;
    } */

    public List<CheepViewModel> GetSelectCheeps(int pageNum)
    {
        var _cheeps = facade.GetCheeps(pageNum);
        return _cheeps;
        /*         {
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
                }  */
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
