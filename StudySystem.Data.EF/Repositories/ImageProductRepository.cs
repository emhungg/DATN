using EFCore.BulkExtensions;
using LinqToDB;
using StudySystem.Data.EF.Repositories.Interfaces;
using StudySystem.Data.Entites;
using StudySystem.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.EF.Repositories
{
    public class ImageProductRepository : Repository<Image>, IImageProductRepository
    {
        private readonly AppDbContext _context;
        public ImageProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        /// <summary>
        /// DeleteImageProduct
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteImageProduct(string productId) 
        {
            var rs = await _context.Set<Image>().Where(x => x.ProductId.Equals(productId)).ToListAsync();
            if (rs.Count() > 0)
            {
                await _context.BulkDeleteAsync(rs).ConfigureAwait(false);
                return true;
            }
            return false;
        }
        /// <summary>
        /// UpdateImageProduct
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="imageName"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> UpdateImageProduct(string productId, List<string> imageName, string user)
        {
            var imgOldDel = await _context.Set<Image>().Where(x => x.ProductId.Equals(productId) && !imageName.Contains(x.ImageDes)).ToListAsync().ConfigureAwait(false);
            if (imgOldDel.Count() > 0)
            {
                await _context.BulkDeleteAsync(imgOldDel).ConfigureAwait(false);
            }
            List<Image> addNew = new List<Image>();
            var imageOldUpdate = await _context.Set<Image>().Where(x => x.ProductId.Equals(productId) && imageName.Contains(x.ImageDes)).ToListAsync().ConfigureAwait(false);
            var listNemImg = imageName.Except(imageOldUpdate.Select(x => x.ImageDes)).ToList();
            foreach (var img in listNemImg)
            {
                string id = StringUtils.NewGuid();
                addNew.Add(new Image { Id = id, ImageDes = img, ProductId = productId, CreateUser = user, UpdateUser = user });
            }
            await _context.BulkInsertAsync(addNew);
            return true;
        }
    }
}
