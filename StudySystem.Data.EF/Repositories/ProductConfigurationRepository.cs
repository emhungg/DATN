using Microsoft.EntityFrameworkCore;
using StudySystem.Data.EF.Repositories.Interfaces;
using StudySystem.Data.Entites;
using StudySystem.Data.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.EF.Repositories
{
    public class ProductConfigurationRepository : Repository<ProductConfiguration>, IProductConfigurationRepository
    {
        private readonly AppDbContext _context;
        public ProductConfigurationRepository(AppDbContext context) : base(context)
        {

            _context = context;

        }
        /// <summary>
        /// DeleteProductConfiguration
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteProductConfiguration(string productId)
        {
            var rs = await _context.Set<ProductConfiguration>().SingleOrDefaultAsync(x => x.ProductId.Equals(productId)).ConfigureAwait(false);
            if (rs != null)
            {
                _context.Remove(rs);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            return false;
        }
        /// <summary>
        /// UpdateProductConfiguration
        /// </summary>
        /// <param name="resquest"></param>
        /// <returns></returns>
        public async Task<bool> UpdateProductConfiguration(UpdateProductRequestModel resquest)
        {
            var rs = await _context.Set<ProductConfiguration>().SingleOrDefaultAsync(x => x.ProductId.Equals(resquest.ProductId)).ConfigureAwait(false);
            if (rs != null)
            {
                rs.Chip = resquest.ChipProduct;
                rs.Screen = resquest.ScreenProduct;
                rs.Rom = resquest.RomProduct;
                rs.Ram = resquest.RamProduct;
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            return false;
        }
    }
}
