using StudySystem.Data.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.EF.Repositories.Interfaces
{
    public interface IAddressUserRepository : IRepository<AddressUser>
    {
        /// <summary>
        /// BulkInsertUsersAddess
        /// </summary>
        /// <param name="addressUser"></param>
        /// <returns></returns>
        Task BulkInsertUsersAddess(List<AddressUser> addressUser);
        /// <summary>
        /// InsertUserAddress
        /// </summary>
        /// <param name="addressUser"></param>
        /// <returns></returns>
        Task<bool> InsertUserAddress(AddressUser addressUser);
    }
}
