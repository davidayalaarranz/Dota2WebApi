using DataModel.Model;
using System.Collections.Generic;

namespace DataModel.ValveJsonModel.Current.GetItems
{
    public class ItemlistResponseModel
    {
        public ItemlistResponseModelResult result { get; set; }
    }

    public class ItemlistResponseModelResult
    {
        public ItemlistResponseModelData data { get; set; }
    }

    public class ItemlistResponseModelData
    {
        public List<HeroItem> itemabilities { get; set; }
    }
}
