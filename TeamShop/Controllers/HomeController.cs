using System;
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

namespace TeamShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Product> Products { get; set; }

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }
        
        public IActionResult Index()
        {
            Products = _db.Products.ToList();
            return View(Products);
        }
        [Route("Home/Index/{id:int}", Name = "showselected")]
        public IActionResult Index(int? id) // do przerobienia na async

        {
            string tag = "";
            List<Product> tagedProduct = new List<Product>();
            switch (id)
            {
                case 0:
                    tag = "psy";
                    Products = Fill.FillProducts(_db.Products.ToList(), tag);
                    break;
                case 1:
                    tag = "koty";
                    Products = Fill.FillProducts(_db.Products.ToList(), tag);
                    break;
                case 2:
                    tag = "inne";
                    Products = Products = Fill.FillProducts(_db.Products.ToList(), tag);
                    break;
                default:
                    Products = _db.Products.ToList();
                    break;
            }
            return View(Products);
        }

        public IActionResult Basket()
        {
            return View();
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
                        mail.From = new System.Net.Mail.MailAddress("cieslik.kamil@outlook.com");
                        mail.Subject = "test";
                        mail.Body = model.Message
                            +"\n========================\nImie i nazwisko: "+model.Name
                            +"\nEmail: "+model.Email+"\nPhone: "
                            +model.Phone
                            +"\nPreferowany sposób komunikacj: "+model.Preferowany;
                        mail.To.Add("cieslik.kamil@outlook.com");

                        using (var smtp = new System.Net.Mail.SmtpClient("smtp-mail.outlook.com", 587) )
                        {
                            smtp.EnableSsl = true;
                            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new NetworkCredential("cieslik.kamil@outlook.com", "igmu6JMFovd7T6");
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
        
    }
}
