using Microsoft.Extensions.Logging;
using StudySystem.Application.Service.Interfaces;
using StudySystem.Data.EF;
using StudySystem.Data.EF.Repositories;
using StudySystem.Data.EF.Repositories.Interfaces;
using StudySystem.Data.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudySystem.Application.Service
{
    public class SupplierService : BaseService, ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISupplierRepository _supplierRepository;
        private readonly ILogger<SupplierService> _logger;
        public SupplierService(IUnitOfWork unitOfWork, ILogger<SupplierService> logger) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _supplierRepository = unitOfWork.SupplierRepository;
            _logger = logger;
        }
        public async Task<SupplierResponseModel> GetSupplierImg()
        {
            SupplierResponseModel supplier = new SupplierResponseModel();
            try
            {
                var rsSupplier = await _supplierRepository.GetSuppliers();

                supplier.Imgs = rsSupplier.Select(x => new SupplierDataModel { Image = x.Image }).ToList();

            }
            catch (Exception ex)
            {

                _logger.LogError(ex.Message);
            }

            return supplier;
        }
    }
}
