using StudySystem.Data.EF.Repositories;
using StudySystem.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Application.Service.Interfaces
{
    public interface ISupplierService : IBaseService
    {
        Task<SupplierResponseModel> GetSupplierImg();
    }
}
