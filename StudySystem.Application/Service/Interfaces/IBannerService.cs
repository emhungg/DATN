using StudySystem.Data.Models.Request;
using StudySystem.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Application.Service.Interfaces
{
    public interface IBannerService : IBaseService
    {
        Task<bool> CreateBanner(BannerDataRequestModel request);

        Task<BannerResponseModel> GetBanner();
        Task<bool> UpdateBanner(int id, bool active);
        Task<bool> DeleteById(int id);
    }

}
