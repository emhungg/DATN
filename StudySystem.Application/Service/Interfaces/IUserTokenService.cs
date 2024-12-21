using StudySystem.Data.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Application.Service.Interfaces
{
    public interface IUserTokenService : IBaseService
    {
        Task<bool> AuthToken(string token);
        Task Delete(string userId);
        Task<UserToken> Insert(UserToken request);
        Task<bool> Update(string userID, DateTime expireTimeOnl);
        Task<bool> IsUserOnl(string userId);
    }
}
