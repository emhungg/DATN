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
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        IUserTokenRepository UserTokenRepository { get; }
        IUserVerificationOTPsRepository UserVerificationOTPsRepository { get; }
        ILocationRepository<Province> ProvioncesRepository { get; }
        ILocationRepository<District> DistrictsRepository { get; }
        ILocationRepository<Ward> WardsRepository { get; }
        IAddressUserRepository AddressUserRepository { get; }
        ISupplierRepository SupplierRepository { get; }
        IProductRepository ProductRepository { get; }
        IProductCategoryRepository ProductCategoryRepository { get; }
        IImageProductRepository ImageProductRepository { get; }
        IProductConfigurationRepository ProductConfigurationRepository { get; }
        ICartItemRepository CartItemRepository { get; }
        ICartRepository CartRepository { get; }
        IOrderRepository OrderRepository { get; }
        IOrderItemRepository OrderItemRepository { get; }
        IRatingProductRepository RatingProductRepository { get; }
        INewsRepository NewsRepository { get; }
        IBannerRepository BannerRepository { get; }
        Task<bool> CommitAsync();
        Task BulkInserAsync<T>(IList<T> entities) where T : class;
        Task BulkUpdateAsync<T>(IList<T> entities) where T : class;
        Task BulkDeleteAsync<T>(IList<T> entities) where T : class;
        IExecutionStrategy CreateExecutionStrategy();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
