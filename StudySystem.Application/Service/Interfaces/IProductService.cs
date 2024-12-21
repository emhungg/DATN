using StudySystem.Data.Models.Request;
using StudySystem.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Application.Service.Interfaces
{
    public interface IProductService : IBaseService
    {
        /// <summary>
        /// CreateProduct
        /// </summary>
        /// <param name="request"></param>
        /// <param name="imageName"></param>
        /// <returns></returns>
        Task<string> CreateProduct(CreateProductRequestModel request, List<string> imageName);
        /// <summary>
        /// GetAllProductDetails
        /// </summary>
        /// <returns></returns>
        Task<ListProductDetailResponseModel> GetAllProductDetails(string hostUrl);
        /// <summary>
        /// UpdateProductDetail
        /// </summary>
        /// <param name="request"></param>
        /// <param name="imageNew"></param>
        /// <returns></returns>
        Task<bool> UpdateProductDetail(UpdateProductRequestModel request, List<string> imageNew);
        /// <summary>
        /// DeleteProduct
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<bool> DeleteProduct(string productId);
        /// <summary>
        /// GetProdcutDetail
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<ProductDetailResponseModel> GetProdcutDetail(string productId);
        /// <summary>
        /// ViewedProduct
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ListProductDetailResponseModel> ViewedProduct(ViewedProductRequestModel request);
        Task<ListProductDetailResponseModel> GetProductByCategoryId(string categoryID);
        /// <summary>
        /// AddWishList
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<string> AddWishList(string productId);

        Task<ListProductDetailResponseModel> GetWishList();
    }
}
