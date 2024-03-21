using Microsoft.AspNetCore.Mvc;

namespace AaronColacoAsp.NETProject.Controllers
{
    public class SalesDataController : Controller
    {
        public IActionResult SalesDashboard()
        {
            return View();
        }
    }
}
