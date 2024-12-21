using StudySystem.Data.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.EF.Repositories.Interfaces
{
    public interface IUserVerificationOTPsRepository : IRepository<VerificationOTP>
    {
        Task<bool> InsertCode(VerificationOTP code);
        Task DeleteCode(string UserID);
    }
}
