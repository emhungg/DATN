using StudySystem.Data.Entites;
using StudySystem.Data.Models.Request;
using StudySystem.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.EF.Repositories.Interfaces
{
    public interface IBannerRepository : IRepository<Banner>
    {
        Task<bool> AddBannerAsync(BannerDataRequestModel banner);
        Task<BannerResponseModel> GetBanner();
        Task<bool> UpdateBanner(int id, bool active);
        Task<bool> DeletebyId(int id);
    }
}
