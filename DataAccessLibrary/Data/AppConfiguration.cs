using DataModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq;

namespace DataAccessLibrary.Data
{
    public class AppConfiguration
    {
        private AppConfiguration () {
            using (Dota2AppDbContext db = new Dota2AppDbContext())
            {
                _items = (from a in db.AppConfiguration select a).ToList();
                string pv = _items.Find(aci => aci.Key.ToUpper().Equals("CurrentDotaPatchVersion".ToUpper())).Value;
                _currentDotaPatchVersion = (from a in db.PatchVersions select a)
                    .Where(x => x.Name == pv)
                    .First();
            }
        }

        private static AppConfiguration _instance;
        private List<AppConfigurationItem> _items;
        private PatchVersion _currentDotaPatchVersion;

        public static string GetValue(string key)
        {
            if (_instance == null)
            {
                _instance = new AppConfiguration();
            }
            return _instance._items.Find(aci => aci.Key.ToUpper().Equals(key.ToUpper())).Value;
        }
        public static PatchVersion CurrentDotaPatchVersion
        {
            get {
                if (_instance == null)
                {
                    _instance = new AppConfiguration();
                }
                return _instance._currentDotaPatchVersion; 
            }
        }
        
        public class AppConfigurationItem
        {
            [Key]
            public string Key { get; set; }
            public string Value { get; set; }
        }
    }
}
