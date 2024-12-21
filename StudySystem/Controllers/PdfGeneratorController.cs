

using Microsoft.AspNetCore.Mvc;
using StudySystem.Data.Models.Response;
using System.Text.Json;

namespace StudySystem.Controllers
{
    public class PdfGeneratorController : Controller
    {
        public IActionResult Index(string model)
        {
            InvoiceResponseModel data = JsonSerializer.Deserialize<InvoiceResponseModel>(model);
            return View(data);
        }
    }
}
