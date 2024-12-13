using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore.Storage;
using StudySystem.Data.EF.Repositories;
using StudySystem.Data.EF.Repositories.Interfaces;
using StudySystem.Data.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private UserRepository _userRegisterRepository;
        private UserTokenRepository _userTokenRepository;
        private UserVerifycationOTPsRepository _userVerifycationOTPsRepository;
        private LocationRepository<Province> _provincesRepository;
        private LocationRepository<District> _districtsRepository;
        private LocationRepository<Ward> _wardsRepository;
        private AddressUserRepository _addressUserRepository;
        private SupplierRepository _supplierRepository;
        private ProductRepository _productRepository;
        private ProductCategoryrepository _productCategoryRepository;
        private ImageProductRepository _imageProductRepository;
        private ProductConfigurationRepository _productConfigurationRepository;
        private CartRepository _cartRepository;
        private CartItemRepository _cartItemRepository;
        private OrderRepository _orderRepository;
        private OrderItemRepository _orderItemRepository;
        private RatingRepository _ratingRepository;
        private NewsRepository _newsRepository;
        private BannerRepository _bannerRepository;
        public UnitOfWork(AppDbContext context) => _context = context;

        public IUserRepository UserRepository
        {
            get { return _userRegisterRepository ?? (_userRegisterRepository = new UserRepository(_context)); }
        }


        public IUserTokenRepository UserTokenRepository
        {
            get { return _userTokenRepository ?? (_userTokenRepository = new UserTokenRepository(_context)); }
        }

        public IUserVerificationOTPsRepository UserVerificationOTPsRepository
        {
            get { return _userVerifycationOTPsRepository ?? (_userVerifycationOTPsRepository = new UserVerifycationOTPsRepository(_context)); }
        }

        public ILocationRepository<Province> ProvioncesRepository
        {
            get { return _provincesRepository ?? (_provincesRepository = new LocationRepository<Province>(_context)); }
        }

        public ILocationRepository<District> DistrictsRepository
        {
            get { return _districtsRepository ?? (_districtsRepository = new LocationRepository<District>(_context)); }
        }

        public ILocationRepository<Ward> WardsRepository
        {
            get { return _wardsRepository ?? (_wardsRepository = new LocationRepository<Ward>(_context)); }
        }

        public IAddressUserRepository AddressUserRepository
        {
            get { return _addressUserRepository ?? (_addressUserRepository = new AddressUserRepository(_context)); }
        }

        public ISupplierRepository SupplierRepository
        {
            get { return _supplierRepository ?? (_supplierRepository = new SupplierRepository(_context)); }
        }

        public IProductRepository ProductRepository
        {
            get { return _productRepository ?? (_productRepository = new ProductRepository(_context)); }
        }


        public IProductCategoryRepository ProductCategoryRepository
        {
            get { return _productCategoryRepository ?? (_productCategoryRepository = new ProductCategoryrepository(_context)); }
        }

        public IImageProductRepository ImageProductRepository
        {
            get { return _imageProductRepository ?? (_imageProductRepository = new ImageProductRepository(_context)); }
        }

        public IProductConfigurationRepository ProductConfigurationRepository
        {
            get { return _productConfigurationRepository ?? (_productConfigurationRepository = new ProductConfigurationRepository(_context)); }
        }

        public ICartRepository CartRepository
        {
            get { return _cartRepository ?? (_cartRepository = new Repositories.CartRepository(_context)); }
        }

        public ICartItemRepository CartItemRepository
        {
            get
            {
                return _cartItemRepository ?? (_cartItemRepository = new Repositories.CartItemRepository(_context));
            }
        }

        public IOrderRepository OrderRepository
        {
            get { return _orderRepository ?? (_orderRepository = new OrderRepository(_context)); }
        }

        public IOrderItemRepository OrderItemRepository
        {
            get
            {
                return _orderItemRepository ?? (_orderItemRepository = new Repositories.OrderItemRepository(_context));
            }
        }

        public IRatingProductRepository RatingProductRepository
        {
            get
            {
                return _ratingRepository ?? (_ratingRepository = new RatingRepository(_context));
            }
        }

        public INewsRepository NewsRepository
        {
            get { return _newsRepository ?? (_newsRepository = new Repositories.NewsRepository(_context)); }
        }

        public IBannerRepository BannerRepository
        {
            get
            {
                return _bannerRepository ?? (_bannerRepository = new Repositories.BannerRepository(_context));
            }
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task BulkDeleteAsync<T>(IList<T> entities) where T : class
        {

            await _context.BulkDeleteAsync(entities).ConfigureAwait(false);

        }

        public async Task BulkInserAsync<T>(IList<T> entities) where T : class
        {
            await _context.BulkInsertAsync(entities).ConfigureAwait(false);
        }

        public async Task BulkUpdateAsync<T>(IList<T> entities) where T : class
        {
            await _context.BulkUpdateAsync(entities).ConfigureAwait(false);
        }

        public async Task<bool> CommitAsync()
        {
            var cm = await _context.SaveChangesAsync().ConfigureAwait(false);
            return cm != 0;
        }

        public IExecutionStrategy CreateExecutionStrategy()
        {
            return _context.Database.CreateExecutionStrategy();
        }
    }
}
