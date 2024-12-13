using EFCore.BulkExtensions;
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
    public class ProductCategoryrepository : Repository<ProductCategory>, IProductCategoryRepository
    {
        private readonly AppDbContext _context;
        public ProductCategoryrepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        
        /// <summary>
        /// UpdateProductCategory
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public async Task<bool> UpdateProductCategory(string productId, string categoryId)
        {
            var productCategory = await _context.Set<ProductCategory>().FirstOrDefaultAsync(x => x.ProductId.Equals(productId));
            if (productCategory == null)
            {
                return false;
            }
            productCategory.CategoryId = categoryId;
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }
    }
}
