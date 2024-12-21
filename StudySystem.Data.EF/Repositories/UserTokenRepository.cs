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
    public class UserTokenRepository : Repository<UserToken>,IUserTokenRepository
    {
        private readonly AppDbContext _context;
        public UserTokenRepository(AppDbContext context) : base(context) 
        {

            _context = context;

        }

        public async Task UpdateStatusActiveToken(string userID)
        {
            var userToken = await _context.Set<UserToken>().SingleOrDefaultAsync(x=>x.UserID.Equals(userID)).ConfigureAwait(false);
            if (userToken != null)
            {
                userToken.IsActive = true;
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
