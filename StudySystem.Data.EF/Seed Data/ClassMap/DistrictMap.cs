using CsvHelper.Configuration;
using StudySystem.Data.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.EF.Seed_Data.ClassMap
{
    public class DistrictMap : ClassMap<District>
    {
        public DistrictMap()
        {
            Map(x => x.Code).Name("Code");
            Map(x => x.Name).Name("Name");
            Map(x => x.NameEn).Name("NameEn");
            Map(x => x.FullName).Name("FullName");
            Map(x => x.FullNameEn).Name("FullNameEn");
            Map(x => x.ProvinceCode).Name("ProvinceCode");
            Map(x => x.AdministrativeUnitId).Name("AdministrativeUnitId");
        }
    }
}
