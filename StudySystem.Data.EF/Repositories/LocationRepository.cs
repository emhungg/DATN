using Microsoft.EntityFrameworkCore;
using StudySystem.Data.EF.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.EF.Repositories
{
    public class LocationRepository<T> : Repository<T>, ILocationRepository<T> where T : class
    {
        private readonly AppDbContext _appDbContext;
        public LocationRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<T>> GetAllLocation()
        {
            return await _appDbContext.Set<T>().Select(x=>x).ToListAsync().ConfigureAwait(false);
        }
    }
}
