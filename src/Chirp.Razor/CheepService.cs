using ViewModel;
using Facade;

namespace Service
{
    public interface ICheepService
    {
        public List<CheepViewModel> GetSelectCheeps(int pageNum);
        public List<CheepViewModel> GetCheepsFromAuthor(string author, int pageNum);
    }

    public class CheepService : ICheepService
    {
        private readonly DBFacade facade;

        public CheepService()
        {
            facade = new DBFacade();
        }

        public List<CheepViewModel> GetSelectCheeps(int pageNum)
        {
            if (pageNum > 0)
            {
                pageNum -= 1;
            }
            var _cheeps = facade.GetCheeps(pageNum);
            return _cheeps;
        }

        public List<CheepViewModel> GetCheepsFromAuthor(string author, int pageNum)
        {
            if (pageNum > 0)
            {
                pageNum -= 1;
            }
            // filter by the provided author name
            var _cheeps = facade.GetCheepsFromAuthor(author, pageNum);
            return _cheeps;
        }
    }
}