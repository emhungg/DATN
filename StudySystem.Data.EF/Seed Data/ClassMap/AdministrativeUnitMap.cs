using CsvHelper.Configuration;
using StudySystem.Data.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.EF.Seed_Data.ClassMap
{
    public class AdministrativeUnitMap : ClassMap<AdministrativeUnit>
    {
        public AdministrativeUnitMap()
        {
            Map(x => x.Id).Name("Id");
            Map(x => x.FullName).Name("FullName");
            Map(x => x.FullNameEn).Name("FullNameEn");
            Map(x => x.ShortName).Name("ShortName");
            Map(x => x.ShortNameEn).Name("ShortNameEn");
            Map(x => x.CodeName).Name("CodeName");
            Map(x => x.CodeNameEn).Name("CodeNameEn");
        }

    }
}
