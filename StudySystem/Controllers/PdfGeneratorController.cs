// <copyright file="PdfGeneratorController.cs" ownedby="Hung">
//  Copyright (c) Hung. All rights reserved.
//  FileType: Visual CSharp source file
//  Created On: 29/09/2023
//  Last Modified On: 29/09/2023
//  Description: PdfGeneratorController.cs
// </copyright>

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
