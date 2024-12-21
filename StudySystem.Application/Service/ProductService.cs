using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using StudySystem.Application.Service.Interfaces;
using StudySystem.Data.EF;
using StudySystem.Data.EF.Repositories.Interfaces;
using StudySystem.Data.Entites;
using StudySystem.Data.Models.Request;
using StudySystem.Data.Models.Response;
using StudySystem.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Application.Service
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductService> _logger;
        private readonly IProductRepository _productRepository;
        private readonly IImageProductRepository _imageProductRepository;
        private readonly IProductCategoryRepository _prodcutCategoryRepository;
        private readonly IProductConfigurationRepository _productConfigurationRepository;
        private readonly string _currentUser;
        public ProductService(IUnitOfWork unitOfWork, ILogger<ProductService> logger, UserResolverSerive user) : base(unitOfWork)
        {

            _unitOfWork = unitOfWork;
            _logger = logger;
            _productRepository = unitOfWork.ProductRepository;
            _currentUser = user.GetUser();
            _imageProductRepository = unitOfWork.ImageProductRepository;
            _prodcutCategoryRepository = unitOfWork.ProductCategoryRepository;
            _productConfigurationRepository = unitOfWork.ProductConfigurationRepository;
        }
        /// <summary>
        /// CreateProduct
        /// </summary>
        /// <param name="request"></param>
        /// <param name="imageName"></param>
        /// <returns></returns>
        public async Task<string> CreateProduct(CreateProductRequestModel request, List<string> imageName)
        {
            var excutionStrategy = _unitOfWork.CreateExecutionStrategy();
            var result = false;
            string rs = "";
            await excutionStrategy.Execute(async () =>
            {
                using (var db = await _unitOfWork.BeginTransactionAsync())
                {
                    try
                    {
                        string productId = StringUtils.NewGuid();
                        if (await ProcessCreateNewProduct(request, productId))
                        {
                            if (await ProcessCreateProductCategory(productId, request.ProductCategoryId))
                            {
                                List<Image> image = new List<Image>();
                                foreach (var item in imageName)
                                {
                                    string imageId = StringUtils.NewGuid();
                                    image.Add(new Image { Id = imageId, ImageDes = item, ProductId = productId, CreateUser = _currentUser, UpdateUser = _currentUser });
                                }
                                if (await ProcessSaveImage(image))
                                {
                                    List<ProductConfiguration> cfg = new List<ProductConfiguration>();
                                    cfg.Add(new ProductConfiguration { ProductId = productId, Chip = request.ChipProduct, Ram = request.RamProduct, Rom = request.RomProduct, Screen = request.ScreenProduct });
                                    if (await ProcessSaveProductConfiguration(cfg))
                                    {
                                        result = true;
                                        rs = productId;
                                        await db.CommitAsync();
                                    }
                                }
                            }
                        }
                        if (!result)
                        {
                            await db.RollbackAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        await db.RollbackAsync();
                    }

                }
            });

            return rs;
        }
        private async Task<bool> ProcessSaveProductConfiguration(List<ProductConfiguration> cfg)
        {
            await _unitOfWork.BulkInserAsync<ProductConfiguration>(cfg);
            return true;
        }
        private async Task<bool> ProcessSaveImage(List<Image> image)
        {
            await _unitOfWork.BulkInserAsync<Image>(image);
            return true;
        }

        private async Task<bool> ProcessCreateProductCategory(string productId, string categoryID)
        {
            try
            {
                List<ProductCategory> productCategories = new List<ProductCategory>();
                productCategories.Add(new ProductCategory { CategoryId = categoryID, ProductId = productId });
                await _unitOfWork.BulkInserAsync<ProductCategory>(productCategories);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
            return false;
        }

        /// <summary>
        /// ProcessCreateNewProduct
        /// </summary>
        /// <param name="request"></param>
        /// <param name="productId"></param>
        /// <returns></returns>
        private async Task<bool> ProcessCreateNewProduct(CreateProductRequestModel request, string productId)
        {
            try
            {
                List<Product> products = new List<Product>();
                products.Add(new Product { ProductId = productId, ProductName = request.ProductName, ProductDescription = request.Description, ProductPrice = request.Price, ProductQuantity = request.ProductQuantity, ProductionDate = request.ProductionDate, BrandName = request.ProductBrand, ProductStatus = request.ProductStatus, PriceSell = request.PriceSell, CreateUser = _currentUser, UpdateUser = _currentUser });
                await _productRepository.CreateProduct(products);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

            }
            return false;
        }

        /// <summary>
        /// GetAllProductDetails
        /// </summary>
        /// <returns></returns>
        public async Task<ListProductDetailResponseModel> GetAllProductDetails(string hostUrl)
        {
            ListProductDetailResponseModel rs = new ListProductDetailResponseModel();
            try
            {
                rs = await _productRepository.GetAllProduct(_currentUser, hostUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return rs;
        }
        /// <summary>
        /// UpdateProductDetail
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> UpdateProductDetail(UpdateProductRequestModel request, List<string> imageNew)
        {
            var excutionStrategy = _unitOfWork.CreateExecutionStrategy();
            var result = false;
            await excutionStrategy.ExecuteAsync(async () =>
            {
                using (var db = await _unitOfWork.BeginTransactionAsync())
                {
                    try
                    {
                        if (await _productRepository.UpdateProduct(request))
                        {
                            if (await _imageProductRepository.UpdateImageProduct(request.ProductId, imageNew, _currentUser))
                            {
                                if (await _productConfigurationRepository.UpdateProductConfiguration(request))
                                {
                                    if (await _prodcutCategoryRepository.UpdateProductCategory(request.ProductId, request.ProductCategoryId))
                                    {
                                        result = true;
                                        await db.CommitAsync();
                                    }
                                }
                            }
                        }
                        if (!result)
                        {
                            await db.RollbackAsync();
                        }

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"{ex.Message}");
                        await db.RollbackAsync();
                    }
                }
            });

            return result;
        }
        /// <summary>
        /// DeleteProduct
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteProduct(string productId)
        {
            var excutionStrategy = _unitOfWork.CreateExecutionStrategy();
            var result = false;
            await excutionStrategy.ExecuteAsync(async () =>
            {
                using (var db = await _unitOfWork.BeginTransactionAsync())
                {
                    try
                    {
                        if (await _productRepository.DeleteProduct(productId))
                        {
                            if (await _imageProductRepository.DeleteImageProduct(productId))
                            {
                                if (await _productConfigurationRepository.DeleteProductConfiguration(productId))
                                {

                                    result = true;
                                    await db.CommitAsync();
                                }
                            }
                        }
                        if (!result)
                        {
                            await db.RollbackAsync();
                        }

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"{ex.Message}");
                        await db.RollbackAsync();
                    }
                }
            });

            return result;
        }
        /// <summary>
        /// GetProdcutDetail
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ProductDetailResponseModel> GetProdcutDetail(string productId)
        {
            var rs = _productRepository.GetProductDetail(productId, _currentUser);
            return (ProductDetailResponseModel)await rs;
        }

        public async Task<ListProductDetailResponseModel> ViewedProduct(ViewedProductRequestModel request)
        {
            ListProductDetailResponseModel rs = new ListProductDetailResponseModel();
            try
            {

                rs = await _productRepository.ViewedProduct(request, _currentUser).ConfigureAwait(false);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
            }

            return rs;
        }

        public async Task<ListProductDetailResponseModel> GetProductByCategoryId(string categoryID)
        {
            ListProductDetailResponseModel rs = new ListProductDetailResponseModel();
            try
            {
                rs = await _productRepository.ProductByCategoryId(categoryID, _currentUser).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message);
                throw;
            }
            return rs;
        }

        public async Task<string> AddWishList(string productId)
        {
            try
            {
                var rs = await _productRepository.AddWishList(_currentUser, productId).ConfigureAwait(false);
                if (rs)
                {
                    return "Thêm";
                }
                else
                {
                    return "Xoá";
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex.Message);
                return "";
            }
        }

        public async Task<ListProductDetailResponseModel> GetWishList()
        {
            try
            {
                return await _productRepository.GetWishList(_currentUser).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
