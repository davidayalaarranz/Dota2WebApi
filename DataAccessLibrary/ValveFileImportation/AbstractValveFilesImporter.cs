using DataAccessLibrary.Data;
using DataModel.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataAccessLibrary.ValveFileImportation
{
    abstract class AbstractValveFilesImporterCreator
    {
        public abstract abstractValveFilesImporter FactoryMethod(string pathJson, Dota2AppDbContext context, PatchVersion cpv);
    }

    public abstract class abstractValveFilesImporter
    {
        public abstractValveFilesImporter(string pathJson, Dota2AppDbContext context, PatchVersion cpv)
        {
            this.pathJson = pathJson;
            this.context = context;
            this.cpv = cpv;
        }

        protected abstract string pathJson { get; set; }
        protected abstract Dota2AppDbContext context { get; set; }
        protected abstract PatchVersion cpv { get; set; }

        public abstract void InitializeAbilities();
        public abstract void InitializeHeroes();
        public abstract void InitializeItems();
    }
}
