using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TeamShop.Data;
using TeamShop.Models;

namespace TeamShop.Controllers
{
/*    [Authorize]*/
    public class SecuredController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ILogger _logger;
        public IEnumerable<Product> Products { get; set; }
        [BindProperty]
        public Product product { get; set; }

        public SecuredController(ApplicationDbContext db, IWebHostEnvironment hostEnvironment, ILogger<SecuredController> logger)
        {
            _db = db;
            webHostEnvironment = hostEnvironment;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult List()
        {
            if (_db.Products == null)
            {
                return View();
            }
            Products = _db.Products.ToList();
            return View(Products);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ProductView model)
        {
                (string,bool) uniqueFileName = UploadedFile(model);
            if (uniqueFileName.Item2)
            {
                Product newProduct = new Product
                {
                    Title = model.Title,
                    Price = model.Price,
                    Quantity = model.Quantity,
                    Tags = model.Tags,
                    Types = model.Types,
                    Description = model.Description,
                    PictureName = uniqueFileName.Item1,
                };
                _db.Add(newProduct);
                await _db.SaveChangesAsync();
            }
            else
            {
                _logger.LogError("Image filed was empty");
            }
            return View();
        }
        private (string,bool) UploadedFile(ProductView model)
        {
            string uniqueFileName = null;
            bool flag = false;
            if (model.Attach1 != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "img");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Attach1.FileName;
                string filePath = Path.Combine(Path.Combine(uploadsFolder, uniqueFileName));
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Attach1.CopyTo(fileStream);
                }
                flag = true;
            }

            return (uniqueFileName,flag);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product product = _db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = _db.Products.Find(id);
            _db.Products.Remove(product);
            _db.SaveChanges();
            return RedirectToAction("List");
        }
    }

}
