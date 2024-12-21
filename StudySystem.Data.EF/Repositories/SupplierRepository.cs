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
    public class SupplierRepository : Repository<Supplier>, ISupplierRepository
    {
        private readonly AppDbContext _appDbContext;
        public SupplierRepository(AppDbContext appDbContext) : base(appDbContext)
        {

            _appDbContext = appDbContext;

        }
        public async Task<List<Supplier>> GetSuppliers()
        {
            return await _appDbContext.Set<Supplier>().Select(x=>x).ToListAsync();
        }
    }
}
