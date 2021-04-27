using DataModel.Model;
using System.Collections.Generic;

namespace DataAccessLibrary.ValveJsonModel.Current.GetItems
{
    public class JsItemsResponseModel
    {
        public Dictionary<string, HeroItem> itemdata { get; set; }
    }
}
