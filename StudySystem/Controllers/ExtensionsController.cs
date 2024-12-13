// <copyright file="ExtensionsController.cs" ownedby="Xuan Truong">
//  Copyright (c) XuanTruong. All rights reserved.
//  FileType: Visual CSharp source file
//  Created On: 29/09/2023
//  Last Modified On: 29/09/2023
//  Description: ExtensionsController.cs
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using NetTopologySuite.Algorithm;
using PuppeteerSharp;
using StudySystem.Application.Service.Interfaces;
using StudySystem.Data.Models.Request;
using StudySystem.Data.Models.Response;
using StudySystem.Middlewares;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Text.Json;
using System.Xml.Linq;


namespace StudySystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExtensionsController : ControllerBase
    {
        private readonly IExtensionsService _extensionsService;
        private readonly ILogger<ExtensionsController> _logger;
        private readonly IBannerService _bannerService;
        private readonly IOrderService _orderService;
        private readonly ISendMailService _sendMailService;
        public ExtensionsController(ILogger<ExtensionsController> logger, IExtensionsService extensionsService, IBannerService bannerService, IOrderService orderService, ISendMailService sendMailService)
        {

            _logger = logger;
            _extensionsService = extensionsService;
            _bannerService = bannerService;
            _orderService = orderService;
            _sendMailService = sendMailService;
        }

        [HttpGet("~/api/price-number")]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> ConvertPriceToNumber(long price)
        {
            try
            {
                var rs = await _extensionsService.ConvertPriceToWords(price);
                return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Data.Models.Response.Response<object>(true, rs));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new BadHttpRequestException("can't convert");
            }


        }

        [HttpPost("~/api/create-banner")]
        [Authorize]
        [AuthPermission]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> CreateBanner([FromForm] BannerDataRequestModel requestModel
            )
        {
            var rs = await _bannerService.CreateBanner(requestModel).ConfigureAwait(false);
            return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Data.Models.Response.Response<object>(rs, new object()));
        }

        [HttpGet("~/api/get-data-banner")]
        public async Task<ActionResult<StudySystemAPIResponse<BannerResponseModel>>> Getbanner()
        {
            var rs = await _bannerService.GetBanner().ConfigureAwait(false);
            return new StudySystemAPIResponse<BannerResponseModel>(StatusCodes.Status200OK, new Data.Models.Response.Response<BannerResponseModel>(true, rs));
        }

        [HttpPost("~/api/update-banner")]
        [Authorize]
        [AuthPermission]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> UpdateBanner(int id, bool active)
        {
            var rs = await _bannerService.UpdateBanner(id, active).ConfigureAwait(false);
            return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Data.Models.Response.Response<object>(rs, new object()));
        }

        [HttpDelete("~/api/delete-id")]
        [Authorize]
        [AuthPermission]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> DeleteById(int id)
        {
            var rs = await _bannerService.DeleteById(id).ConfigureAwait(false);
            return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Data.Models.Response.Response<object>(rs, new object()));
        }

        [HttpGet("~/api/invoice-bill")]
        public async Task<IActionResult> ReturnInvoices(string orderID)
        {
            try
            {
                var data = await _orderService.GetInvoice(orderID);
                var rs = await _extensionsService.ConvertPriceToWords(long.Parse(data.ToTalAmount.ToString()));
                data.AmountInWords = rs;

                var options = new LaunchOptions
                {
                    Headless = true,
                };

                var browserFetcher = new BrowserFetcher();
                await browserFetcher.DownloadAsync();

                using var browser = await Puppeteer.LaunchAsync(options);
                using var page = await browser.NewPageAsync();
                using var memoryStream = new MemoryStream();

                var url = Url.ActionLink("Index", "PdfGenerator", new { model = JsonSerializer.Serialize(data) });
                await page.GoToAsync(url);

                var pdfStream = await page.PdfDataAsync();

                return File(pdfStream, "application/pdf", "Invoice.pdf");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }


        [HttpGet("~/api/generate-link")]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> GenerateLink(string orderID)
        {
            var request = HttpContext.Request;
            var currentUrl = $"{request.Scheme}://{request.Host}";
            var data = await _orderService.GetInvoice(orderID);
            var rs = await _extensionsService.ConvertPriceToWords(long.Parse(data.ToTalAmount.ToString()));
            data.AmountInWords = rs;
            var url = Url.Action("Index", "PdfGenerator", new { model = JsonSerializer.Serialize(data) });
            return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Data.Models.Response.Response<object>(true, currentUrl + url));

        }

        /// <summary>
        /// API send noti to email user when order done
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        [HttpPost("~/api/noti-mail-order-done")]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> NotiEmailOredrDone(NotiOrderDoneRequestModel data)
        {
            string body = System.IO.File.ReadAllText("./Views/MailOrder/MailOrders.html");
            var rs = await _sendMailService.NotiEmailOrderDone(body, data);
            return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Data.Models.Response.Response<object>(rs, new object()));
        }

    }
}
