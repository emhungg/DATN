
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudySystem.Application.Service.Interfaces;
using StudySystem.Data.Models.Response;
using StudySystem.Infrastructure.CommonConstant;

namespace StudySystem.Controllers
{
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }
        /// <summary>
        /// <para>api: ~/api/supplier-img</para>
        /// GetSupplierImg
        /// </summary>
        /// <returns></returns>
        [HttpGet(Router.SupplierImg)]
        public async Task<ActionResult<StudySystemAPIResponse<SupplierResponseModel>>> GetSupplierImg()
        {
            var rs = await _supplierService.GetSupplierImg().ConfigureAwait(false);
            return new StudySystemAPIResponse<SupplierResponseModel>(StatusCodes.Status200OK, new Response<SupplierResponseModel>(true, rs));
        }
    }
}
