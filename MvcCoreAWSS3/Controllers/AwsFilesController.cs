using Microsoft.AspNetCore.Mvc;
using MvcCoreAWSS3.Services;

namespace MvcCoreAWSS3.Controllers
{
    public class AwsFilesController : Controller
    {
        private ServiceStorageS3 service;

        public AwsFilesController(ServiceStorageS3 service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            List<string> files = await this.service.GetAllFilesAsync();
            ViewData["MENSAJE"] = TempData["MENSAJE"];
            return View(files);
        }

        public async Task<IActionResult> DeleteFile(string fileName)
        {
            await this.service.DeleteFileAsync(fileName);
            return RedirectToAction("Index");
        }

        public IActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            int codigo = 0;
            using (Stream stream = file.OpenReadStream())
            {
                codigo = await this.service.UploadFileAsync(file.FileName, stream);
            }
            TempData["MENSAJE"] = "Http Status Code: " + codigo;
            return RedirectToAction("Index");
        }
    }
}
