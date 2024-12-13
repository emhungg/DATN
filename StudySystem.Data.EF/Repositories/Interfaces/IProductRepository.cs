using StudySystem.Data.Entites;
using StudySystem.Data.Models.Data;
using StudySystem.Data.Models.Request;
using StudySystem.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.EF.Repositories.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task CreateProduct(List<Product> products);
        Task<ListProductDetailResponseModel> GetAllProduct(string userId, string hostUrl);
        Task<bool> UpdateProduct(UpdateProductRequestModel updateProduct);
        Task<bool> DeleteProduct(string productId);
        Task<ProductDetailResponseModel> GetProductDetail(string productId, string userId);
        Task<ListProductDetailResponseModel> ViewedProduct(ViewedProductRequestModel request, string userId);
        Task<ListProductDetailResponseModel> ProductByCategoryId(string categoryId, string userId);
        Task<bool> UpdateProductQuantity(UpdateProductQuantityDataModel data);

        Task<bool> AddWishList(string userId, string productId);
        /// <summary>
        /// GetWishList
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ListProductDetailResponseModel> GetWishList(string userId);
    }
}
