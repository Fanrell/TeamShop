using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeamShop.Data;
using TeamShop.Models;

namespace TeamShop.Controllers
{
    [Authorize]
    public class SecuredController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment webHostEnvironment;
        public Product product { get; set; }

        public SecuredController(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            webHostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ProductView model)
        {
                string uniqueFileName = UploadedFile(model);
                Product newProduct = new Product
                {
                    Title = model.Title,
                    Price = model.Price,
                    Quantity = model.Quantity,
                    Tags = model.Tags,
                    Types = model.Types,
                    Description = model.Description,
                    PictureName = uniqueFileName,
                };
            _db.Add(newProduct);
            await _db.SaveChangesAsync();
            return View();
        }
        private string UploadedFile(ProductView model)
        {
            string uniqueFileName = null;

                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "img");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Attach1.FileName;
                string filePath = Path.Combine(Path.Combine(uploadsFolder, uniqueFileName));
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Attach1.CopyTo(fileStream);
                }

            return uniqueFileName;
        }
    }

}
