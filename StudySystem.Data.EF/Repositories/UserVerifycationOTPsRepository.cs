using Microsoft.EntityFrameworkCore;
using StudySystem.Data.EF.Repositories.Interfaces;
using StudySystem.Data.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.EF.Repositories
{
    public class UserVerifycationOTPsRepository : Repository<VerificationOTP>, IUserVerificationOTPsRepository
    {
        private readonly AppDbContext _context;
        public UserVerifycationOTPsRepository(AppDbContext context) : base(context)
        {

            _context = context;

        }

        public async Task DeleteCode(string UserID)
        {
            var query = await _context.Set<VerificationOTP>().SingleOrDefaultAsync(x => x.UserID.Equals(UserID)).ConfigureAwait(false);
            if (query != null)
            {
                _context.Set<VerificationOTP>().Remove(query);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<bool> InsertCode(VerificationOTP code)
        {
            await _context.Set<VerificationOTP>().AddAsync(code).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }
    }
}
