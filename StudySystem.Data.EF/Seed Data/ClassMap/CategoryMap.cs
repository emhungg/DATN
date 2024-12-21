using CsvHelper.Configuration;
using StudySystem.Data.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.EF.Seed_Data.ClassMap
{
    public class CategoryMap : ClassMap<Category>
    {

        public CategoryMap()
        {
            Map(x => x.CategoryId).Name("CategoryId");
            Map(x => x.CategoryName).Name("CategoryName");
            Map(x => x.UpdateUser).Name("UpdateUser");
            Map(x => x.CreateUser).Name("CreateUser");
            Map(x => x.CreateDateAt).Name("CreateDateAt");
            Map(x => x.UpdateDateAt).Name("UpdateDateAt");
        }
    }
}
