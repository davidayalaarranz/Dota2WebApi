using BusinessLibrary.Model;
using DataModel.Model;
using System.Threading.Tasks;

namespace BusinessLibrary.Service
{
    public interface IHeroItemService
    {
        Task<HeroItemResponseModel> GetHeroItems();
        Task<HeroItem> GetHeroItem(int id);
        Task<HeroItemResponseModel> GetHeroItems(DataTableParameters p);
    }
}
