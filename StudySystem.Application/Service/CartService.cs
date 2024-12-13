using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Math.EC.Rfc7748;
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
    public class CartService : BaseService, ICartService
    {
        private readonly ILogger<CartService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _currentUser;
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductRepository _productRepository;
        public CartService(ILogger<CartService> logger, IUnitOfWork unitOfWork, UserResolverSerive currentUser) : base(unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser.GetUser();
            _cartRepository = unitOfWork.CartRepository;
            _cartItemRepository = unitOfWork.CartItemRepository;
            _productRepository = unitOfWork.ProductRepository;
        }

        public async Task<bool> DeleteCart(CartItemsRequestModel model)
        {
            try
            {
                var cartId = await _cartRepository.FindAsync(x => x.UserId.Equals(_currentUser)).ConfigureAwait(false);
                var cartItem = await _cartItemRepository.FindAllAsync(x => x.CartId.Equals(cartId.CartId)).ConfigureAwait(false);
                var rs = cartItem.Where(x => model.CartInsertData.Select(x => x.ProductId).Contains(x.ProductId)).ToList();
                List<CartItem> cartItems = new List<CartItem>();
                cartItems = rs;
                await _unitOfWork.BulkDeleteAsync(cartItems).ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }

        }

        /// <summary>
        /// GetCart
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<CartResponseModel> GetCart()
        {
            CartResponseModel cartResponseModel = new CartResponseModel();
            try
            {
                var rs = _cartRepository.GetCart(_currentUser);
                cartResponseModel = rs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return cartResponseModel;
        }
        /// <summary>
        /// InsertOrUpdateCart
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> InsertOrUpdateCart(CartItemsRequestModel model)
        {
            try
            {
                string cartId = StringUtils.NewGuid();
                var cartUser = await _cartRepository.InsertCartUser(_currentUser, cartId).ConfigureAwait(false);
                if (cartUser == null)
                {
                    await ProcessUpdateCartItem(model, cartId);
                }
                else
                {
                    await ProcessUpdateCartItem(model, cartUser.CartId);
                }
                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }

        }
        /// <summary>
        /// UpdateQuantity
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> UpdateQuantity(CartUpdateDataModel model)
        {
            try
            {
                var cart = await _cartRepository.FindAsync(x => x.UserId.Equals(_currentUser)).ConfigureAwait(false);
                var cartItem = await _cartItemRepository.FindAsync(x => x.ProductId.Equals(model.ProductId) && x.CartId.Equals(cart.CartId)).ConfigureAwait(false);
                if (cartItem != null)
                {
                    cartItem.Quantity = model.Quantity;
                    await _unitOfWork.CommitAsync();
                }
                return model.Quantity;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
            }
            return 0;
        }



        /// <summary>
        /// ProcessUpdateCartItem
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cartId"></param>
        /// <returns></returns>
        private async Task ProcessUpdateCartItem(CartItemsRequestModel model, string cartId)
        {
            List<CartItem> cartItems = new List<CartItem>();
            try
            {
                var oldItem = await _cartItemRepository.FindAllAsync(x => x.CartId.Equals(cartId));
                var oldItemDictory = oldItem.ToDictionary(x => x.ProductId);
                foreach (var item in model.CartInsertData)
                {
                    int quantityNew = 0;
                    var thisProduct = await _productRepository.FindAsync(x => x.ProductId.Equals(item.ProductId)).ConfigureAwait(false);
                    if (oldItemDictory.TryGetValue(item.ProductId, out var existingItem))
                    {
                        quantityNew = existingItem.Quantity + item.Quantity;
                    }
                    else
                    {
                        quantityNew = item.Quantity;

                    }
                    if (quantityNew <= thisProduct.ProductQuantity)
                    {
                        cartItems.Add(new CartItem
                        {
                            CartId = cartId,
                            ProductId = item.ProductId,
                            Quantity = quantityNew,
                            Price = item.Price,
                            CreateUser = _currentUser,
                            UpdateUser = _currentUser
                        });
                    }

                }
                await _unitOfWork.BulkUpdateAsync(cartItems).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

        }
    }
}
