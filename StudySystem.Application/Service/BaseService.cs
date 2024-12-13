
using StudySystem.Application.Service.Interfaces;
using StudySystem.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Application.Service
{
    /// <summary>
    /// Base service
    /// </summary>
    public class BaseService : IBaseService
    {
        protected IUnitOfWork UnitOfWork { get; set; }
        /// <summary>
        /// Baseservice
        /// </summary>
        /// <param name="unitOfWork"></param>
        protected BaseService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
    }
}
