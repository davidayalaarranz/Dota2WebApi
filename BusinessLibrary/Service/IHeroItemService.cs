using BusinessLibrary.Model;
using DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLibrary.Service
{
    public class DataTableParameters
    {
        public int start { get; set; } = 0;
        public int length { get; set; } = 0;
        public string orderBy { get; set; } = "";
        public string order { get; set; } = "asc";
        public string filter { get; set; } = "";
    }

    public interface IHeroItemService
    {
        Task<HeroItemResponseModel> GetHeroItems();
        Task<HeroItem> GetHeroItem(int id);
        Task<HeroItemResponseModel> GetHeroItems(DataTableParameters p);
    }
}
