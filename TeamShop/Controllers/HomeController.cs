using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TeamShop.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using TeamShop.Data;
using TeamShop.Tools;
using System.Net.Mail;
using System.Net;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace TeamShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        /*private readonly IHttpContextAccessor _httpContextAccessor;*/
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public string ShopingCartId { get; set; }
        public const string CartSessionKey = "CartID"; 

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }
        
        public IActionResult Index()
        {
            if(_db.Products == null)
            {
                return View();
            }
            Products = _db.Products.ToList();
            return View(Products);
        }
        [Route("Home/Index/{id:int}", Name = "showselected",Order = 1)]
        public IActionResult Index(int? id) // do przerobienia na async

        {
            string[] tags = { "psy", "koty", "inne" };
            Products = _db.Products.Where(p => p.Tags == tags[(int)id]);
            return View(Products);
        }
        [HttpGet]
        [Route("Home/Index/{id:int}/{id2:int}", Name = "showselectedfilter", Order = 2)]
        public IActionResult Index(int? id, int? id2) // do przerobienia na async

        {
            string[] tags = { "psy", "koty", "inne" };
            string[] categories = { "Karmy", "Zabawki", "Akcesoria", "Inne" };
            Products = _db.Products.Where(p => p.Tags == tags[(int)id] && p.Types == categories[(int)id2]);
            return View(Products);
        }


        public IActionResult Basket()
        {
            List<Product> productsList = new List<Product>();
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("cart")))
            {
                var fromSession = HttpContext.Session.GetString("cart");
                productsList = JsonSerializer.Deserialize<List<Product>>(fromSession);
            }
            Products = productsList;
            return View(Products);
        }

        public IActionResult Catalog(int? id)
        {
            
            Product product = _db.Products.FirstOrDefault(u => u.Id == id);
            if (product == null)
                return NotFound();
            return View(product);
        }

        public IActionResult Account()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost, ActionName("AddToCart")]
        public IActionResult AddToCart(int id)
        {
            List<Product> listProducts = new List<Product>();
            Product product = _db.Products.FirstOrDefault(i => i.Id == id);
            if(string.IsNullOrEmpty(HttpContext.Session.GetString("cart")))
            {
                HttpContext.Session.SetString("cart", JsonSerializer.Serialize(listProducts));
            }
            var fromSession = HttpContext.Session.GetString("cart");
            listProducts = JsonSerializer.Deserialize<List<Product>>(fromSession);
            listProducts.Add(product);
            HttpContext.Session.SetString("cart", JsonSerializer.Serialize(listProducts));
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("DeleteFromCart")]
        public IActionResult DeleteFromCart(int id)
        {
            List<Product> listProducts = new List<Product>();
            Product product = _db.Products.FirstOrDefault(i => i.Id == id);
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("cart")))
            {
                HttpContext.Session.SetString("cart", JsonSerializer.Serialize(listProducts));
            }
            var fromSession = HttpContext.Session.GetString("cart");
            listProducts = JsonSerializer.Deserialize<List<Product>>(fromSession);
            ListRemove.Remove(ref listProducts, id);
            HttpContext.Session.SetString("cart", JsonSerializer.Serialize(listProducts));
            return RedirectToAction("Basket");
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string ReturnUrl)
        {
            Users = _db.Users.ToList();
            foreach (var item in Users)
            {
                if ((username == item.Login) && (password == item.Password))
                {
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username)
                };
                    var claimsIdentity = new ClaimsIdentity(claims, "Login");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    return Redirect(ReturnUrl == null ? "/Secured" : ReturnUrl);
                }
            }
            return View();
        }

        [HttpPost]
        public IActionResult SendEmail(EmailModel model)
        {


            try
            {
                if (ModelState.IsValid)
                {
                    using (var mail = new System.Net.Mail.MailMessage())
                    {
                        mail.From = new System.Net.Mail.MailAddress("teamshoplapa@outlook.com", "Sklep Zoologiczny Łapa");
                        mail.Subject = "test";
                        mail.Body = model.Message
                            +"\n========================\nImie i nazwisko: "+model.Name
                            +"\nEmail: "+model.Email+"\nPhone: "
                            +model.Phone
                            +"\nPreferowany sposób komunikacj: "+model.Preferowany;
                        mail.To.Add(mail.From.Address);
                        mail.To.Add(model.Email);

                        using (var smtp = new System.Net.Mail.SmtpClient("smtp-mail.outlook.com", 587) )
                        {
                            smtp.EnableSsl = true;
                            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new NetworkCredential("teamshoplapa@outlook.com", "nxvcZ8437XR4C9");
                            ServicePointManager.ServerCertificateValidationCallback =
                                (sender, certificate, chain, sslPolicyErrors) => true;

                            smtp.Send(mail);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
            return RedirectToAction("Contact");
        }

        [HttpPost]
        public IActionResult AddToBracket(Product product)
        {
            return View("Index");
        }
        
    }
}
