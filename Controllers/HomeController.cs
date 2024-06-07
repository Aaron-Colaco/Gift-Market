using AaronColacoAsp.NETProject.Models;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace AaronColacoAsp.NETProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public static void SendEmailToCusotmer(string userEmail, string htmlcodeforbody = null, string subject = null)
        {
            var AdminEmail = new MailAddress("AaronShopMvc@outlook.com", "Shop App");
            var Cusotmer = new MailAddress(userEmail, "Dear Customer");
            var Adminpassword = "Bucket@23";

            var Emailbody = htmlcodeforbody;
            var Emailsubject = "GIFT MARKET NZ";


          


            var smtp = new SmtpClient
            {
                Host = "smtp.office365.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(AdminEmail.Address, Adminpassword)
            };

            using (var emailcontent = new MailMessage(AdminEmail, Cusotmer)
            {
                Subject = Emailsubject,
                Body = htmlcodeforbody,
                IsBodyHtml = true


            })
            {
                smtp.Send(emailcontent);
            }

            return;


        }



        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
