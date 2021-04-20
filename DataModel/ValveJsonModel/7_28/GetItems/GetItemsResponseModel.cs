using DataModel.Model;
using System.Collections.Generic;

namespace DataModel.ValveJsonModel.v7_28.GetItems
{
    public class GetItemsResponseModel
    {
        public GetItemsResponseModel() : base()
        {
        }
        public GetItemsResponseModelResult result {
            get; 
            set; }
    }

    public class GetItemsResponseModelResult
    {
        public List<HeroItem> items { get; 
            set; }
        public int status { get; 
            set; }
    }
}
