using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Data.EF.Repositories.Interfaces
{
    public interface ILocationRepository<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// GetAllLocation
        /// </summary>
        /// <returns></returns>
        public Task<List<T>> GetAllLocation();
    }
}
