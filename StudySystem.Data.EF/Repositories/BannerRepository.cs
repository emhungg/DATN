using Dapper;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using StudySystem.Data.EF.Repositories.Interfaces;
using StudySystem.Data.Entites;
using StudySystem.Data.Models.Request;
using StudySystem.Data.Models.Response;
using StudySystem.Infrastructure.Configuration;
using StudySystem.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.EF.Repositories
{
    public class BannerRepository : Repository<Banner>, IBannerRepository
    {
        private readonly AppDbContext _context;
        public BannerRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        /// <summary>
        /// AddBannerAsync
        /// </summary>
        /// <param name="banner"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> AddBannerAsync(BannerDataRequestModel banner)
        {
            Banner bannerNew = new Banner
            {
                Image = ImageConverter.ConvertToBase64(banner.Image),
                Title = banner.Title,
                IsActive = banner.IsActive
            };
            await _context.AddAsync(bannerNew).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }

        public async Task<bool> DeletebyId(int id)
        {
            var query = await _context.Set<Banner>().FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            if (query != null)
            {
                _context.Remove(query);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            return false;
        }


        /// <summary>
        /// GetBanner
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<BannerResponseModel> GetBanner()
        {
            using (var connection = new NpgsqlConnection(AppSetting.ConnectionString))
            {
                // Mở kết nối
                await connection.OpenAsync();

                // Truy vấn SQL trực tiếp với Dapper
                var query = await connection.QueryAsync<BannerDataModel>(@"SELECT ""Id"", ""Title"", ""Image"", ""IsActive"" as isActive,(""CreateDateAt"" + INTERVAL '7 hours')  as CreateAt FROM ""Banners"" ");

                BannerResponseModel rs = new BannerResponseModel
                {
                    Data = query.ToList()
                };
                await connection.CloseAsync();
                return rs;
            }
        }

        /// <summary>
        /// UpdateBanner
        /// </summary>
        /// <param name="id"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> UpdateBanner(int id, bool active)
        {
            var query = await _context.Set<Banner>().FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            if (query != null)
            {
                query.IsActive = active;
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            return false;
        }
    }
}
