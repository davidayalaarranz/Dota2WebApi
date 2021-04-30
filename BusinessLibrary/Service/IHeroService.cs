using BusinessLibrary.Model;
using DataModel.Model;
using System.Threading.Tasks;

namespace BusinessLibrary.Service
{
    public interface IHeroService
    {
        Task<Hero> GetHero(int id);
        Task<HeroResponseModel> GetHeroes(DataTableParameters p, PatchVersion pv = null);
    }
}
