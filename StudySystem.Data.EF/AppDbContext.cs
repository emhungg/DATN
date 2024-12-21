using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudySystem.Data.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.EF
{
    public class AppDbContext : DbContext
    {
        private readonly ILogger<AppDbContext> _logger;
        private readonly string _currentUser;
        public AppDbContext(DbContextOptions<AppDbContext> options, UserResolverSerive userResoveSerive, ILogger<AppDbContext> logger) : base(options)
        {
            _logger = logger;
            _currentUser = userResoveSerive.GetUser();
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<UserDetail> UserDetails => Set<UserDetail>();
        public DbSet<VerificationOTP> VerificationOTPs => Set<VerificationOTP>();
        public DbSet<UserToken> UserTokens => Set<UserToken>();
        public DbSet<AdministrativeRegion> AdministrativeRegions => Set<AdministrativeRegion>();
        public DbSet<AdministrativeUnit> AdministrativeUnits => Set<AdministrativeUnit>();
        public DbSet<Province> Provinces => Set<Province>();
        public DbSet<District> Districts => Set<District>();
        public DbSet<Ward> Wards => Set<Ward>();
        public DbSet<AddressUser> AddressUsers => Set<AddressUser>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<WishList> WishLists => Set<WishList>();
        public DbSet<News> News => Set<News>();
        public DbSet<Banner> Banners => Set<Banner>();
        public DbSet<Image> Images => Set<Image>();
        public DbSet<ProductConfiguration> ProductConfigurations => Set<ProductConfiguration>();
        public DbSet<AddressBook> AddressBooks => Set<AddressBook>();
        public DbSet<RatingProduct> RatingProduct => Set<RatingProduct>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            #region UserDetail
            modelBuilder.Entity<UserDetail>(cfg =>
            {
                cfg.HasKey(x => x.UserID);
            });
            // Create an index for the 'UserID' column in the 'UserDetail' table
            modelBuilder.Entity<UserDetail>()
                .HasIndex(ud => ud.UserID)
                .HasName("IX_UserDetail_UserID");
            #endregion

            #region VerificationOTP
            modelBuilder.Entity<VerificationOTP>(cfg =>
            {
                cfg.HasKey(cfg => cfg.UserID);
            });
            #endregion

            #region User token
            modelBuilder.Entity<UserToken>(cfg =>
            {
                cfg.HasKey(cfg => cfg.UserID);
            });
            #endregion

            #region administrative_regions
            modelBuilder.Entity<AdministrativeRegion>(cfg =>
            {
                cfg.HasKey(x => x.Id);
            });

            #endregion

            #region administrative_units
            modelBuilder.Entity<AdministrativeUnit>(cfg =>
            {
                cfg.HasKey(x => x.Id);
            });

            #endregion

            #region Provinces
            // define pk, fk
            modelBuilder.Entity<Province>().HasKey(x => x.Code);
            // AdministrativeRegions
            modelBuilder.Entity<Province>()
           .HasOne(p => p.AdministrativeRegion)
           .WithMany(p => p.Provinces)
           .HasForeignKey(p => p.AdministrativeRegionId).OnDelete(DeleteBehavior.Restrict);
            // AdministrativeUnits
            modelBuilder.Entity<Province>()
                .HasOne(p => p.AdministrativeUnit)
                .WithMany(p => p.Provinces)
                .HasForeignKey(p => p.AdministrativeUnitId).OnDelete(DeleteBehavior.Restrict);
            // define index
            modelBuilder.Entity<Province>()
                .HasIndex(x => new { x.AdministrativeRegionId, x.AdministrativeUnitId });
            #endregion

            #region Districts
            modelBuilder.Entity<District>().HasKey(x => x.Code);
            modelBuilder.Entity<District>()
           .HasOne(d => d.AdministrativeUnit)
           .WithMany(d => d.Districts)
           .HasForeignKey(d => d.AdministrativeUnitId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<District>()
                .HasOne(d => d.Province)
                .WithMany(d => d.Districts)
                .HasForeignKey(d => d.ProvinceCode).OnDelete(DeleteBehavior.Restrict);
            // define index
            modelBuilder.Entity<District>()
                .HasIndex(x => new { x.ProvinceCode, x.AdministrativeUnitId });
            #endregion

            #region Wards
            modelBuilder.Entity<Ward>().HasKey(x => x.Code);
            modelBuilder.Entity<Ward>()
            .HasOne(w => w.AdministrativeUnit)
            .WithMany(w => w.Wards)
            .HasForeignKey(w => w.AdministrativeUnitId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ward>()
                .HasOne(w => w.District)
                .WithMany(w => w.Wards)
                .HasForeignKey(w => w.DistrictCode).OnDelete(DeleteBehavior.Restrict);
            // define index
            modelBuilder.Entity<Ward>()
                .HasIndex(x => new { x.DistrictCode, x.AdministrativeUnitId });
            #endregion

            #region Address user
            modelBuilder.Entity<AddressUser>().HasKey(x => x.Id);
            // Define foreign key relationships 1 - 1 user - address
            modelBuilder.Entity<UserDetail>()
                .HasOne(x => x.AddressUser)
                .WithOne(x => x.UserDetail)
                .HasForeignKey<AddressUser>(x => x.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            // Define foreign key relationships n - 1 : addressUsers - ward
            modelBuilder.Entity<AddressUser>()
                .HasOne(x => x.Ward)
                .WithMany(x => x.AddressUsers)
                .HasForeignKey(x => x.WardCode).OnDelete(DeleteBehavior.Restrict);

            // Define foreign key relationships n - 1 : addressUsers - district
            modelBuilder.Entity<AddressUser>()
                .HasOne(x => x.District)
                .WithMany(x => x.AddressUsers)
                .HasForeignKey(x => x.DistrictCode).OnDelete(DeleteBehavior.Restrict);

            // Define foreign key relationships n - 1 : addressUsers - province
            modelBuilder.Entity<AddressUser>()
                .HasOne(x => x.Province)
                .WithMany(x => x.AddressUsers)
                .HasForeignKey(x => x.ProvinceCode).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AddressUser>()
                .HasIndex(x => new { x.UserID, x.WardCode, x.DistrictCode, x.ProvinceCode });
            #endregion

            #region product
            modelBuilder.Entity<Product>().HasKey(e => e.ProductId);
            modelBuilder.Entity<Product>().HasIndex(e => e.ProductId);
            #endregion

            #region category
            modelBuilder.Entity<Category>().HasKey(e => e.CategoryId);
            modelBuilder.Entity<Category>().HasIndex(e => e.CategoryId);
            #endregion

            #region product category
            modelBuilder.Entity<ProductCategory>().HasKey(e => new { e.ProductId, e.CategoryId });

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(pc => pc.ProductId);

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Category)
                .WithMany(c => c.ProductCategories)
                .HasForeignKey(pc => pc.CategoryId);
            #endregion

            #region cart
            modelBuilder.Entity<Cart>().HasKey(x => x.CartId);
            modelBuilder.Entity<Cart>()
                .HasOne(x => x.UserDetail)
                .WithMany(x => x.Carts)
                .HasForeignKey(x => x.UserId);
            #endregion

            #region cartitem
            modelBuilder.Entity<CartItem>().HasKey(e => new { e.CartId, e.ProductId });

            modelBuilder.Entity<CartItem>()
           .HasOne(c => c.Cart)
           .WithMany(ci => ci.CartItems)
           .HasForeignKey(ci => ci.CartId);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductId);
            #endregion

            #region order
            modelBuilder.Entity<Order>().HasKey(e => e.OrderId);
            modelBuilder.Entity<Order>()
            .HasOne(u => u.UserDetail)
            .WithMany(o => o.Orders)
            .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<Order>()
            .HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId);
            #endregion

            #region order item
            modelBuilder.Entity<OrderItem>().HasKey(e => new { e.OrderId, e.ProductId });
            // n-1 between OrderItem and Product
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId);
            #endregion

            #region supplier
            modelBuilder.Entity<Supplier>().HasKey(e => e.Id);
            #endregion

            #region wishList
            modelBuilder.Entity<WishList>()
                .HasOne(x => x.UserDetail)
                .WithMany(e => e.WishLists)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<WishList>()
                .HasOne(x => x.Product)
                .WithMany(e => e.WishLists)
                .HasForeignKey(x => x.ProductId);
            #endregion

            #region news
            modelBuilder.Entity<News>(cfg =>
            {
                cfg.HasKey(cfg => cfg.Id);
            });
            #endregion

            #region Banner
            modelBuilder.Entity<Banner>(cfg =>
            {
                cfg.HasKey(cfg => cfg.Id);
            });
            #endregion

            #region image
            modelBuilder.Entity<Image>().HasKey(e => new { e.Id });
            modelBuilder.Entity<Image>().HasIndex(e => e.ProductId);
            #endregion

            #region product configuration
            modelBuilder.Entity<ProductConfiguration>().HasKey(e => e.ProductId);
            #endregion



            #region address book
            modelBuilder.Entity<AddressBook>().HasKey(x => x.OrderId);
            #endregion

            #region rating
            modelBuilder.Entity<RatingProduct>().HasKey(x => new { x.UserId, x.ProductId });
            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }

    public class UserResolverSerive
    {
        private readonly IHttpContextAccessor _context;
        private readonly ILogger<UserResolverSerive> _logger;
        public UserResolverSerive(IHttpContextAccessor context, ILogger<UserResolverSerive> logger)
        {
            _context = context;
            _logger = logger;
        }
        public string GetUser()
        {
            try
            {
                var user = _context.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "UserID")?.Value;
                return user ?? "";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return "";
            }
        }
        public string GetIpAdressUser()
        {
            string ipAddress = string.Empty;
            try
            {
                IPAddress ip = _context.HttpContext.Connection.RemoteIpAddress;
                if (ipAddress != null)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                    {
                        ip = Dns.GetHostEntry(ip).AddressList.First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                    }
                    ipAddress = ip.ToString();
                }
                return ipAddress;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return "";
            }
        }
    }
}
