// <copyright file="ProductController.cs" ownedby="Xuan Truong">
//  Copyright (c) XuanTruong. All rights reserved.
//  FileType: Visual CSharp source file
//  Created On: 29/09/2023
//  Last Modified On: 29/09/2023
//  Description: ProductController.cs
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using StudySystem.Application.Service;
using StudySystem.Application.Service.Interfaces;
using StudySystem.Data.Models.Data;
using StudySystem.Data.Models.Request;
using StudySystem.Data.Models.Response;
using StudySystem.Infrastructure.CommonConstant;
using StudySystem.Infrastructure.Resources;
using StudySystem.Middlewares;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace StudySystem.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly IProductService _productService;
        private readonly IRatingService _ratingService;
        public ProductController(ILogger<ProductController> logger, IWebHostEnvironment environment, IProductService productService, IRatingService ratingService)
        {
            _logger = logger;
            _environment = environment;
            _productService = productService;
            _ratingService = ratingService;
        }

        /// <summary>
        /// CreateProduct
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost(Router.CreateProduct)]
        //[Authorize]
        //[AuthPermission]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> CreateProduct([FromForm] CreateProductRequestModel request)
        {
            List<string> pdImgs = ListImageName(request.ImageProducts);
            var rs = await _productService.CreateProduct(request, pdImgs);
            if (rs == "")
            {
                throw new BadHttpRequestException(Message.ErrorCreateProduct);
            }
            else
            {
                try
                {
                    string Filepath = GetFilepath(rs);
                    if (!System.IO.Directory.Exists(Filepath))
                    {
                        System.IO.Directory.CreateDirectory(Filepath);
                    }
                    foreach (var file in request.ImageProducts)
                    {
                        string imagepath = Filepath + "\\" + file.FileName;
                        if (System.IO.File.Exists(imagepath))
                        {
                            System.IO.File.Delete(imagepath);
                        }
                        using (FileStream stream = System.IO.File.Create(imagepath))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }

            }
            return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Response<object>(true, rs));
        }

        /// <summary>
        /// GetAllProduct
        /// api: "~/api/get-product"
        /// </summary>
        /// <returns></returns>
        [HttpGet(Router.GetAllProduct)]
        public async Task<ActionResult<StudySystemAPIResponse<ListProductDetailResponseModel>>> GetAllProduct()
        {
            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/Product/";
            var rs = await _productService.GetAllProductDetails(hosturl);
  
            return new StudySystemAPIResponse<ListProductDetailResponseModel>(StatusCodes.Status200OK, new Response<ListProductDetailResponseModel>(true, rs));
        }

        /// <summary>
        /// UpdateProduct
        /// "~/api/update-product"
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPut(Router.UpdateProduct)]
        //[Authorize]
        //[AuthPermission]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> UpdateProduct([FromForm] UpdateProductRequestModel requestModel)
        {
            List<string> pdImgs = ListImageName(requestModel.ImageProducts);
            var rs = await _productService.UpdateProductDetail(requestModel, pdImgs);
            if (rs)
            {
                await MultiRemove(requestModel);
            }
            else
            {
                throw new BadHttpRequestException(Message.ErrorCreateProduct);
            }
            return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Response<object>(rs, new object()));
        }

        /// <summary>
        /// DeleteProduct
        /// api: "~/api/delete-product"
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        /// <exception cref="BadHttpRequestException"></exception>
        [HttpDelete(Router.DeleteProduct)]
        [Authorize]
        [AuthPermission]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> DeleteProduct(string productId)
        {
            var rs = await _productService.DeleteProduct(productId);
            if (!rs)
            {
                throw new BadHttpRequestException(Message.ErrorCreateProduct);
            }
            else
            {
                var filePath = GetFilepath(productId);
                if (System.IO.Directory.Exists(filePath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(filePath);
                    FileInfo[] fileInfos = directoryInfo.GetFiles();

                    // Xóa những file có trong thư mục mà không có trong request.ImageProducts
                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        fileInfo.Delete();
                    }
                    System.IO.Directory.Delete(filePath);
                }
            }
            return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Response<object>(rs, new object()));
        }
        /// <summary>
        /// GetProductDetail
        /// "~/api/product-details"
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet(Router.GetProductDetail)]
        public async Task<ActionResult<StudySystemAPIResponse<ProductDetailResponseModel>>> GetProductDetail(string productId)
        {
            var rs = await _productService.GetProdcutDetail(productId);
            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/Product/";
            foreach (var image in rs.Images)
            {
                image.ImagePath = hosturl + $"{rs.ProductId}/" + image.ImagePath;
            }
            return new StudySystemAPIResponse<ProductDetailResponseModel>(StatusCodes.Status200OK, new Response<ProductDetailResponseModel>(true, rs));
        }
        /// <summary>
        /// ListViewedProduct
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("~/api/viewed-product")]
        public async Task<ActionResult<StudySystemAPIResponse<ListProductDetailResponseModel>>> ListViewedProduct(ViewedProductRequestModel request)
        {
            var rs = await _productService.ViewedProduct(request);
            string hosturl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/Product/";
            if (rs != null)
            {
                foreach (var productDetail in rs.listProductDeatails)
                {
                    foreach (var image in productDetail.Images)
                    {
                        image.ImagePath = hosturl + $"{productDetail.ProductId}/" + image.ImagePath;
                    }
                }
            }

            return new StudySystemAPIResponse<ListProductDetailResponseModel>(StatusCodes.Status200OK, new Response<ListProductDetailResponseModel>(true, rs));
        }

        /// <summary>
        /// GeProductByCategoryId
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("~/api/product-by-category")]
        public async Task<ActionResult<StudySystemAPIResponse<ListProductDetailResponseModel>>> GeProductByCategoryId(string request)
        {
            var rs = await _productService.GetProductByCategoryId(request);
            string hosturl = $"{this.Request.Scheme}:/{this.Request.Host}{this.Request.PathBase}/Product/";
            foreach (var productDetail in rs.listProductDeatails)
            {
                foreach (var image in productDetail.Images)
                {
                    image.ImagePath = hosturl + $"{productDetail.ProductId}/" + image.ImagePath;
                }
            }
            return new StudySystemAPIResponse<ListProductDetailResponseModel>(StatusCodes.Status200OK, new Response<ListProductDetailResponseModel>(true, rs));
        }


        [HttpPost("~/api/wish-product")]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> WishChangeProduct(string productId)
        {
            var rs = await _productService.AddWishList(productId);

            return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Response<object>(true, rs));
        }

        /// <summary>
        /// GetWishList
        /// </summary>
        /// <returns></returns>
        [HttpGet("~/api/get-wish-list")]
        public async Task<ActionResult<StudySystemAPIResponse<ListProductDetailResponseModel>>> GetWishList()
        {
            var rs = await _productService.GetWishList();
            string hosturl = $"{this.Request.Scheme}:/{this.Request.Host}{this.Request.PathBase}/Product/";
            if (rs != null)
            {
                foreach (var productDetail in rs.listProductDeatails)
                {
                    foreach (var image in productDetail.Images)
                    {
                        image.ImagePath = hosturl + $"{productDetail.ProductId}/" + image.ImagePath;
                    }
                }
            }
            return new StudySystemAPIResponse<ListProductDetailResponseModel>(StatusCodes.Status200OK, new Response<ListProductDetailResponseModel>(true, rs));
        }

        /// <summary>
        /// AddCommnetRating
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        /// <exception cref="BadHttpRequestException"></exception>
        [HttpPost("~/api/add-comment-rating")]
        [Authorize]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> AddCommnetRating(RatingRequestModel requestModel)
        {
            var rs = await _ratingService.AddCommentRating(requestModel);
            if (!rs)
            {
                throw new BadHttpRequestException(Message.ErrorCreateProduct);
            }
            return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Response<object>(rs, new object()));
        }

        /// <summary>
        /// GetCommnetRating
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("~/api/get-comment-rating")]
        public async Task<ActionResult<StudySystemAPIResponse<RatingResponseModel>>> GetCommnetRating(string productId)
        {
            var rs = await _ratingService.GetRatingProductById(productId);
            return new StudySystemAPIResponse<RatingResponseModel>(StatusCodes.Status200OK, new Response<RatingResponseModel>(true, rs));
        }

        [NonAction]
        private List<string> ListImageName(IFormFileCollection objFile)
        {
            List<string> list = new List<string>();
            try
            {
                foreach (var file in objFile)
                {
                    list.Add(file.FileName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }
            return list;
        }

        [NonAction]
        private string GetFilepath(string productId)
        {
            return _environment.WebRootPath + "\\product\\" + productId;
        }
        [NonAction]
        private async Task<bool> MultiRemove(UpdateProductRequestModel request)
        {
            try
            {
                string Filepath = GetFilepath(request.ProductId);
                if (System.IO.Directory.Exists(Filepath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(Filepath);
                    FileInfo[] fileInfos = directoryInfo.GetFiles();

                    // Xóa những file có trong thư mục mà không có trong request.ImageProducts
                    foreach (FileInfo fileInfo in fileInfos)
                    {
                        string imagepath = Filepath + "\\" + fileInfo.Name;
                        if (!request.ImageProducts.Any(file => file.FileName == fileInfo.Name))
                        {
                            System.IO.File.Delete(imagepath);
                        }
                    }

                    // Thêm những file có trong request.ImageProducts mà không có trong thư mục
                    foreach (var file in request.ImageProducts)
                    {
                        string imagepath = Filepath + "\\" + file.FileName;
                        if (!System.IO.File.Exists(imagepath))
                        {
                            using (FileStream stream = System.IO.File.Create(imagepath))
                            {
                                await file.CopyToAsync(stream);
                            }
                        }

                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }



    }
}
