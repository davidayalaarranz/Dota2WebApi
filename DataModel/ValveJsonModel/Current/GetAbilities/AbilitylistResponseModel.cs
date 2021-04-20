using DataModel.Model;
using System.Collections.Generic;

namespace DataModel.ValveJsonModel.Current.GetAbilities
{
    public class AbilitylistResponseModel
    {
        public AbilitylistResponseModelResult result { get; set; }
    }

    public class AbilitylistResponseModelResult
    {
        public AbilitylistResponseModelData data { get; set; }
    }

    public class AbilitylistResponseModelData
    {
        public List<Ability> itemabilities { get; set; }
    }
}
