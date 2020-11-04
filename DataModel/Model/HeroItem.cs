using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Model
{
    public class HeroItemComponent
    {
        public HeroItemComponent() { }

        public long HeroItemId { get; set; }
        public HeroItem HeroItem { get; set; }
        public long ComponentId { get; set; }
        public HeroItem Component { get; set; }
        public int Quantity
        {
            get;
            set;
        }
    }
    public class HeroItem
    {
        public HeroItem()
        {
            Components = new List<HeroItemComponent>();
            IsComponentOf = new List<HeroItemComponent>();
            Description = string.Empty;
            Notes = string.Empty;
            Lore = string.Empty;
            Attrib = string.Empty;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long HeroItemId { get; set; }

        public string Name { get; set; }
        public string ShortName { get { return Name.Remove(0, 5); } }
        public int Cost { get; set; }
        public bool IsSecretShop { get; set; }
        public bool IsSideShop { get; set; }
        public bool IsRecipe { get; set; }
        public string LocalizedName { get; set; }
        public string ImageUrl { get { 
                if (IsRecipe)
                    return string.Concat("https://cdn.cloudflare.steamstatic.com/apps/dota2/images/items/recipe_lg.png");
                else
                    return string.Concat("https://cdn.cloudflare.steamstatic.com/apps/dota2/images/items/", ShortName, "_lg.png"); } 
        }
        public int Cooldown { get; set; }
        public int ManaCost { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string Lore { get; set; }
        public List<HeroItemComponent> Components { get; set; }
        public List<HeroItemComponent> IsComponentOf { get; set; }
        public bool Created { get; set; }
        public string Attrib { get; set; }
    }
}
