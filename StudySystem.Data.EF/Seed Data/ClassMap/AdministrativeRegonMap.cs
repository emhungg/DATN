using CsvHelper.Configuration;
using StudySystem.Data.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.EF.Seed_Data.ClassMap
{
    public class AdministrativeRegonMap : ClassMap<AdministrativeRegion>
    {
        public AdministrativeRegonMap()
        {
            Map(m => m.Id).Name("Id");
            Map(m => m.Name).Name("Name");
            Map(m => m.NameEn).Name("NameEn");
            Map(m => m.CodeName).Name("CodeName");
            Map(m => m.CodeNameEn).Name("CodeNameEn");
        }
    }
}
