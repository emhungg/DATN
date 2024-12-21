using Dapper;
using EFCore.BulkExtensions;
using LinqToDB.Data;
using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using StudySystem.Data.EF.Repositories.Interfaces;
using StudySystem.Data.Entites;
using StudySystem.Data.Models.Data;
using StudySystem.Data.Models.Request;
using StudySystem.Data.Models.Response;
using StudySystem.Infrastructure.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static LinqToDB.Common.Configuration;

namespace StudySystem.Data.EF.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context) : base(context)
        {

            _context = context;

        }

        public async Task<bool> AddWishList(string userId, string productId)
        {
            var qeury = await _context.WishLists.SingleOrDefaultAsync(x => x.UserId.Equals(userId) && x.ProductId.Equals(productId)).ConfigureAwait(false);
            if (qeury != null)
            {
                _context.WishLists.Remove(qeury);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return false;
            }
            else
            {
                WishList addNew = new WishList();
                addNew.UserId = userId;
                addNew.ProductId = productId;
                await _context.AddAsync(addNew).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }

        }

        /// <summary>
        /// CreateProduct
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        public async Task CreateProduct(List<Product> products)
        {
            await _context.BulkInsertOrUpdateAsync(products);
        }
        /// <summary>
        /// DeleteProduct
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteProduct(string productId)
        {
            var rs = await _context.Set<Product>().SingleOrDefaultAsync(x => x.ProductId.Equals(productId)).ConfigureAwait(false);
            if (rs != null)
            {
                _context.Remove(rs);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            return false;
        }
        /// <summary>
        /// GetAllProduct
        /// </summary>
        /// <returns></returns>
        public async Task<ListProductDetailResponseModel> GetAllProduct(string userId, string hostUrl)
        {
            IEnumerable<Image> imageProductData = await new ImageProductRepository(_context).GetAllAsNoTrackingAsyn().ConfigureAwait(false);
            IEnumerable<ProductCategory> categories = await new ProductCategoryrepository(_context).GetAllAsNoTrackingAsyn().ConfigureAwait(false);
            List<Category> categories1 = await _context.Categories.AsNoTracking().Select(x => x).ToListAsyncEF().ConfigureAwait(false);

            IEnumerable<ProductDetailResponseModel> data = _context.Products.AsNoTracking().AsEnumerable().Select(x => new ProductDetailResponseModel()
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                ProductDescription = x.ProductDescription,
                ProductPrice = x.ProductPrice,
                ProductBrand = x.BrandName,
                ProductQuantity = x.ProductQuantity,
                ProductSell = x.PriceSell,
                ProductionDate = x.ProductionDate,
                ProductStatus = x.ProductStatus,
                CategoryId = categories.SingleOrDefault(c => c.ProductId == x.ProductId).CategoryId,
                ProductCategory = "",
                // Sử dụng dữ liệu ảnh đã tải để gán vào từng sản phẩm
                Images = imageProductData.Where(i => i.ProductId == x.ProductId)
                                 .Select(i => new ImageProductData
                                 {
                                     ImagePath = hostUrl + x.ProductId + "/" + i.ImageDes
                                 })
                                 .ToList(),
                ProductConfig = new ProductConfigData { Chip = "", Ram = 0, Rom = 0, Screen = "" },
                IsLike = _context.WishLists.AsNoTracking().Any(w => w.UserId.Equals(userId) && w.ProductId.Equals(x.ProductId))
            }
            );
            foreach (var item in data)
            {
                item.ProductCategory = categories1.SingleOrDefault(x => x.CategoryId == item.CategoryId).CategoryName;
            }
            ListProductDetailResponseModel model = new ListProductDetailResponseModel();
            model.listProductDeatails = data.ToList();
            return model;
        }

        public async Task<ProductDetailResponseModel> GetProductDetail(string productId, string userId)
        {
            IEnumerable<Image> imageProductData = await new ImageProductRepository(_context).GetAllAsNoTrackingAsyn().ConfigureAwait(false);
            IEnumerable<ProductCategory> categories = await new ProductCategoryrepository(_context).GetAllAsNoTrackingAsyn().ConfigureAwait(false);
            List<Category> categories1 = await _context.Categories.AsNoTracking().Select(x => x).ToListAsyncEF().ConfigureAwait(false);
            ProductConfiguration productConfiguration = await _context.ProductConfigurations.AsNoTracking().SingleOrDefaultAsync(x => x.ProductId.Equals(productId)).ConfigureAwait(false);

            var data = _context.Products.AsNoTracking().AsEnumerable().Select(x => new ProductDetailResponseModel()
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                ProductDescription = x.ProductDescription,
                ProductPrice = x.ProductPrice,
                ProductBrand = x.BrandName,
                ProductQuantity = x.ProductQuantity,
                ProductSell = x.PriceSell,
                ProductionDate = x.ProductionDate,
                ProductStatus = x.ProductStatus,
                CategoryId = categories.SingleOrDefault(c => c.ProductId == x.ProductId).CategoryId,
                ProductCategory = "",
                // Sử dụng dữ liệu ảnh đã tải để gán vào từng sản phẩm
                Images = imageProductData.Where(i => i.ProductId == x.ProductId)
                                  .Select(i => new ImageProductData
                                  {
                                      ImagePath = i.ImageDes
                                  })
                                  .ToList(),
                ProductConfig = new ProductConfigData() { Chip = productConfiguration.Chip, Ram = productConfiguration.Ram, Rom = productConfiguration.Rom, Screen = productConfiguration.Screen },
                IsLike = _context.WishLists.AsNoTracking().Any(w => w.UserId.Equals(userId) && w.ProductId.Equals(x.ProductId))
            }
             ).SingleOrDefault(x => x.ProductId.Equals(productId));

            return data;
        }
        /// <summary>
        /// GetWishList
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ListProductDetailResponseModel> GetWishList(string userId)
        {
            var result = (from wl in _context.WishLists
                          join p in _context.Products on wl.ProductId equals p.ProductId
                          join pc in _context.ProductCategories on p.ProductId equals pc.ProductId
                          join i in _context.Images on p.ProductId equals i.ProductId
                          where wl.UserId.Equals(userId)
                          group new { p, i } by new { p.ProductId, p.ProductName, p.ProductDescription, p.ProductPrice, p.BrandName, p.ProductQuantity, p.ProductionDate, p.ProductStatus, p.PriceSell } into pGroup
                          select new ProductDetailResponseModel
                          {
                              ProductId = pGroup.Key.ProductId,
                              ProductName = pGroup.Key.ProductName,
                              ProductPrice = pGroup.Key.ProductPrice,
                              ProductSell = pGroup.Key.PriceSell,
                              ProductStatus = pGroup.Key.ProductStatus,
                              Images = new List<ImageProductData> { pGroup.Select(x => new ImageProductData { ImagePath = x.i.ImageDes }).FirstOrDefault() },
                              ProductBrand = pGroup.Key.BrandName,
                              IsLike = _context.WishLists.Any(x => x.UserId.Equals(userId) && x.ProductId.Equals(pGroup.Key.ProductId))
                          }).ToListAsync().ConfigureAwait(false);
            ListProductDetailResponseModel listProductDetailResponseModel = new ListProductDetailResponseModel();
            listProductDetailResponseModel.listProductDeatails = await result;
            return listProductDetailResponseModel;

        }

        public async Task<ListProductDetailResponseModel> ProductByCategoryId(string categoryId, string userId)
        {
            IEnumerable<Image> imageProductData = await new ImageProductRepository(_context).GetAllAsNoTrackingAsyn().ConfigureAwait(false);
            using (var connection = new NpgsqlConnection(AppSetting.ConnectionString))
            {
                // Mở kết nối
                await connection.OpenAsync();

                // Truy vấn SQL trực tiếp với Dapper
                var query = await connection.QueryAsync<ProductDetailResponseModel>(@"
                              select ""ProductId"" as ProductId,
                            ""ProductName"" as ProductName,
                             ""ProductPrice"" as ProductPrice,
                             ""PriceSell"" as ProductSell,
                             ""ProductStatus"" as ProductStatus,
                             null as Images,
                             ""BrandName"" as ProductBrand,
                            case when
	                            exists (select 1 from ""WishLists"" wl where p.""ProductId"" = wl.""ProductId"" and wl.""UserId"" = @userId  limit 1)
                            then true else false end as IsLike 
                            from ""Products"" p where ""ProductId"" in (select ""ProductId"" from ""ProductCategories"" where ""CategoryId"" = @categoryId)
                ", new { categoryId = categoryId, userId = userId });

                await connection.CloseAsync();

                foreach (var item in query)
                {
                    item.Images = imageProductData.Where(i => i.ProductId == item.ProductId)
                                  .Select(i => new ImageProductData
                                  {
                                      ImagePath = i.ImageDes
                                  })
                                  .ToList();
                }

                return new ListProductDetailResponseModel()
                {
                    listProductDeatails = query.ToList()
                };
            }

        }

        /// <summary>
        /// UpdateProduct
        /// </summary>
        /// <param name="updateProduct"></param>
        /// <returns></returns>
        public async Task<bool> UpdateProduct(UpdateProductRequestModel updateProduct)
        {
            var rs = await _context.Set<Product>().FirstOrDefaultAsync(x => x.ProductId.Equals(updateProduct.ProductId)).ConfigureAwait(false);
            if (rs != null)
            {
                rs.ProductName = updateProduct.ProductName;
                rs.ProductPrice = updateProduct.Price;
                rs.PriceSell = updateProduct.PriceSell;
                rs.ProductDescription = updateProduct.Description;
                rs.ProductQuantity = updateProduct.ProductQuantity;
                rs.ProductionDate = updateProduct.ProductionDate;
                rs.ProductStatus = updateProduct.ProductStatus;
                rs.BrandName = updateProduct.ProductBrand;
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateProductQuantity(UpdateProductQuantityDataModel data)
        {
            int count = 0;
            foreach (var item in data.ProductChangedData)
            {
                var productChange = await _context.Set<Product>().FirstOrDefaultAsync(x => x.ProductId.Equals(item.ProductId)).ConfigureAwait(false);
                if (productChange != null)
                {
                    productChange.ProductQuantity = productChange.ProductQuantity - item.Quantity;
                    count++;
                }
            }
            if (count == data.ProductChangedData.Count())
            {
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            return false;

        }

        public async Task<ListProductDetailResponseModel> ViewedProduct(ViewedProductRequestModel request, string userId)
        {
            var result = (from p in _context.Products
                          join pc in _context.ProductCategories on p.ProductId equals pc.ProductId
                          join i in _context.Images on p.ProductId equals i.ProductId
                          where request.ProductIdData.Select(k => k.ProductId).Contains(p.ProductId)
                          group new { p, i } by new { p.ProductId, p.ProductName, p.ProductDescription, p.ProductPrice, p.BrandName, p.ProductQuantity, p.ProductionDate, p.ProductStatus, p.PriceSell } into pGroup
                          select new ProductDetailResponseModel
                          {
                              ProductId = pGroup.Key.ProductId,
                              ProductName = pGroup.Key.ProductName,
                              ProductPrice = pGroup.Key.ProductPrice,
                              ProductSell = pGroup.Key.PriceSell,
                              ProductStatus = pGroup.Key.ProductStatus,
                              Images = pGroup.Select(x => new ImageProductData { ImagePath = x.i.ImageDes }).ToList(),
                              IsLike = _context.WishLists.Any(x => x.UserId.Equals(userId) && x.ProductId.Equals(pGroup.Key.ProductId))
                          }).ToListAsync().ConfigureAwait(false);
            ListProductDetailResponseModel listProductDetailResponseModel = new ListProductDetailResponseModel();
            listProductDetailResponseModel.listProductDeatails = await result;
            return listProductDetailResponseModel;
        }
    }
}
