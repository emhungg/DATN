using Microsoft.EntityFrameworkCore;
using StudySystem.Data.EF.Repositories.Interfaces;
using StudySystem.Data.Entites;
using StudySystem.Data.Models.Data;
using StudySystem.Data.Models.Request;
using StudySystem.Infrastructure.CommonConstant;
using StudySystem.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.EF.Repositories
{
    public class UserRepository : Repository<UserDetail>, IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        /// <summary>
        /// ChangeAddress
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> ChangeAddress(string userId, ChangeAddressRequestModel request)
        {
            var findAddress = await _context.AddressUsers.FirstOrDefaultAsync(x => x.UserID.Equals(userId));
            if (findAddress != null)
            {
                findAddress.WardCode
                    = request.WardCode;
                findAddress.DistrictCode = request.DistrictCode;
                findAddress.Descriptions = request.Descriptions;
                findAddress.ProvinceCode = request.ProvinceCode;
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            return false;
        }

        public async Task<bool> ChangeNameGender(string userId, ChangeNameGenderRequestModel request)
        {
            var query = await _context.Set<UserDetail>().SingleOrDefaultAsync(x => x.UserID.Equals(userId.ToLower())).ConfigureAwait(false);
            if (query != null)
            {
                query.UserFullName = request.Name.Trim();
                query.Gender = request.Gender;
                query.UpdateDateAt = DateTime.UtcNow;
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            return false;
        }
        /// <summary>
        /// ChangePassword
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="oldPassword"></param>
        /// <param name="passwordNew"></param>
        /// <returns></returns>
        public async Task<bool> ChangePassword(string userId, string oldPassword, string passwordNew)
        {
            var query = await _context.Set<UserDetail>().SingleOrDefaultAsync(x => x.UserID.Equals(userId.ToLower())).ConfigureAwait(false);
            if (PasswordHasher.VerifyPassword(oldPassword, query.Password))
            {
                query.Password = PasswordHasher.HashPassword(passwordNew);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            return false;
        }


        /// <summary>
        /// Admin
        /// DeleteUserInfor
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeleteUserInfor(string userId)
        {
            var query = await _context.Set<UserDetail>().SingleOrDefaultAsync(x => x.UserID.Equals(userId.ToLower())).ConfigureAwait(false);
            var address = await _context.Set<AddressUser>().FirstOrDefaultAsync(x => x.UserID.Equals(userId.ToLower())).ConfigureAwait(false);
            if (query != null)
            {
                _context.Remove(query);
                if (address != null)
                {
                    _context.Remove(address);
                }
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            return false;
        }
        /// <summary>
        /// GetAllUsersInfo
        /// </summary>
        /// <returns></returns>
        public IQueryable<UsersInforDataModel> GetAllUsersInfo()
        {
            var query = (from user in _context.Set<UserDetail>().AsNoTracking()
                         join address in _context.Set<AddressUser>().AsNoTracking()
                         on user.UserID equals address.UserID
                         join province in _context.Set<Province>().AsNoTracking()
                         on address.ProvinceCode equals province.Code
                         join district in _context.Set<District>().AsNoTracking()
                        on address.DistrictCode equals district.Code
                         join ward in _context.Set<Ward>().AsNoTracking()
                         on address.WardCode equals ward.Code
                         where !user.UserID.Contains(CommonConstant.UserIdSession)
                         select new UsersInforDataModel
                         {
                             UserId = user.UserID,
                             UserName = user.UserFullName,
                             Email = user.Email,
                             PhoneNumber = user.PhoneNumber,
                             Gender = user.Gender,
                             Address = address.Descriptions + " " + ward.Name + ", " + district.Name + ", " + province.Name,
                             Role = user.Role,
                             IsActive = user.IsActive,
                             CreateDateAt = user.CreateDateAt.ToString("MM/dd/yyyy"),
                             RankUser = StringUtils.RankUser(_context.Orders.Where(x => x.UserId.Equals(user.UserID) && x.StatusReceive == OrderStatusReceive.IsShipped).Select(x => Convert.ToDecimal(x.TotalAmount)).Sum()),
                         });
            return query;
        }

        /// <summary>
        /// GetUserDetailById
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<UserDetailDataModel> GetUserDetail(string userId)
        {
            var address = await (from au in _context.Set<AddressUser>()
                                 join wa in _context.Set<Ward>() on au.WardCode equals wa.Code
                                 join dt in _context.Set<District>() on au.DistrictCode equals dt.Code
                                 join pr in _context.Set<Province>() on au.ProvinceCode equals pr.Code
                                 where au.UserID == userId
                                 select new
                                 {
                                     au,
                                     wa,
                                     dt,
                                     pr
                                 }).FirstOrDefaultAsync().ConfigureAwait(false);

            var result = await (from ud in _context.UserDetails
                                join o in _context.Orders on ud.UserID equals o.UserId into userOrders
                                from uo in userOrders.DefaultIfEmpty()
                                join oi in _context.OrderItems on uo.OrderId equals oi.OrderId into userOrderItems
                                from uoi in userOrderItems.DefaultIfEmpty()
                                where ud.UserID == userId
                                group new { uoi } by new { ud.UserFullName, ud.Email, ud.PhoneNumber, uo.Status, uo.StatusReceive, ud.Gender, ud.CreateDateAt, uoi.Quantity } into g
                                select new
                                {
                                    UserFullName = g.Key.UserFullName,
                                    Email = g.Key.Email,
                                    PhoneNumber = g.Key.PhoneNumber,
                                    PriceBought = g.Where(x => g.Key.StatusReceive == OrderStatusReceive.IsShipped).Sum(x => x.uoi.Price),
                                    
                                    Gender = g.Key.Gender == 0 ? "Nam" : "Nữ",
                                    JoinDateAt = g.Key.CreateDateAt.ToString("dd/MM/yyyy"),
                                    CountOrderItem = g.Sum(x => x.uoi != null ? x.uoi.Quantity : 0)
                                }).ToListAsync();

            var userDetailDataModel = new UserDetailDataModel
            {
                UserFullName = result.FirstOrDefault()?.UserFullName,
                Email = result.FirstOrDefault()?.Email,
                PhoneNumber = result.FirstOrDefault()?.PhoneNumber,
                Gender = result.FirstOrDefault()?.Gender,
                JoinDateAt = result.FirstOrDefault()?.JoinDateAt,
                PriceBought = result.Sum(x => x.PriceBought),
                CountOrderItem = result.Sum(x => x.CountOrderItem),
                AddressUserDes = address.au.Descriptions,
                AddressUserWard = address.wa.Name,
                WardCode = address.wa.Code,
                DistrictCode = address.dt.Code,
                ProvinceCode = address.pr.Code,
                AddressUserDistrict = address.dt.Name,
                AddressUserProvince = address.pr.Name,
                RankUser = StringUtils.RankUser(result.Sum(x => x.PriceBought)),
            };


            return userDetailDataModel;
        }

        /// <summary>
        /// InsertUserDetails
        /// </summary>
        /// <param name="userDetail"></param>
        /// <returns></returns>
        public async Task<bool> InsertUserDetails(UserDetail userDetail)
        {
            var check = await _context.Set<UserDetail>().SingleOrDefaultAsync(x => x.UserID.Equals(userDetail.UserID.ToLower())).ConfigureAwait(false);
            if (check != null)
            {
                _context.Set<UserDetail>().Remove(check);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            await _context.Set<UserDetail>().AddAsync(userDetail).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }
        /// <summary>
        /// IsUserExists
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> IsUserExists(string userId)
        {
            var query = await _context.Set<UserDetail>().SingleOrDefaultAsync(x => x.UserID.Equals(userId.ToLower())).ConfigureAwait(false);
            return query != null;
        }
        /// <summary>
        /// UpdateStatusActiveUser
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task UpdateStatusActiveUser(string userID)
        {
            var query = await _context.Set<UserDetail>().SingleOrDefaultAsync(x => x.UserID.Equals(userID.ToLower())).ConfigureAwait(false);
            if (query != null)
            {
                query.IsActive = true;
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Admin
        /// UpdateUserInfor
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleNew"></param>
        /// <param name="activeNew"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> UpdateUserInfor(string userId, int roleNew, bool activeNew)
        {
            var query = await _context.Set<UserDetail>().SingleOrDefaultAsync(x => x.UserID.Equals(userId.ToLower())).ConfigureAwait(false);
            if (query != null)
            {
                query.IsActive = activeNew;
                query.Role = roleNew;
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            return false;
        }
    }
}
