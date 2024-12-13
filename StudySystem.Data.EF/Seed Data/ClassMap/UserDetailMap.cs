using CsvHelper.Configuration;
using StudySystem.Data.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.EF.Seed_Data.ClassMap
{
    public class UserDetailMap : ClassMap<UserDetail>
    {
        public UserDetailMap()
        {
            Map(m => m.UserID).Name("UserID");
            Map(m => m.UserFullName).Name("UserFullName");
            Map(m => m.Email).Name("Email");
            Map(m => m.Password).Name("Password");
            Map(m => m.PhoneNumber).Name("PhoneNumber");
            Map(m => m.Gender).Name("Gender");
            Map(m => m.Role).Name("Role");
            Map(m => m.IsActive).Name("IsActive");
        }
    }
}
