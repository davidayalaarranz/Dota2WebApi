using DataModel.Model;
using System.Collections.Generic;

namespace DataAccessLibrary.ValveJsonModel.Current.GetAbilities
{
    public class AbilitydetailResponseModel
    {
        public AbilitydetailResponseModelResult result { get; set; }
    }

    public class AbilitydetailResponseModelResult
    {
        public AbilitydetailResponseModelData data { get; set; }
    }

    public class AbilitydetailResponseModelData
    {
        public List<Ability> abilities { get; set; }
    }
}
