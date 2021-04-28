using DataModel.Model;
using System.Collections.Generic;

namespace DataAccessLibrary.ValveJsonModel.Current.GetItems
{
    public class ItemDetailResponseModel
    {
        public ItemDetailResponseModelResult result { get; set; }
    }

    public class ItemDetailResponseModelResult
    {
        public ItemDetailResponseModelData data { get; set; }
    }

    public class ItemDetailResponseModelData
    {
        public List<HeroItem> items { get; set; }
    }
}
