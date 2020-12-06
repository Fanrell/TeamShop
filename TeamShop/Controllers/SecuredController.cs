using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        public Product product { get; set; }

        public SecuredController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string title, float price, int quantity, string tags, string types, string description, IFormFile attach1)
        {
            product = new Product();
            product.Title = title;
            product.Price = price;
            product.Quantity = quantity;
            product.Tags = tags;
            product.Types = types;
            product.Description = description;
            attach1.CopyTo(product.Attach1);
            _db.Products.Add(product);
            _db.SaveChanges();
            return View();
        }
    }

}
