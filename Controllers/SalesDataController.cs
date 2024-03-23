using AaronColacoAsp.NETProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AaronColacoAsp.NETProject.Controllers
{
    public class SalesDataController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SalesDataController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult SalesDashboard()
        {

            var OrderData = _context.Order;
            var OrderItemData = _context.OrderItem;

            decimal TotalSales = OrderData.Sum(a => a.TotalPrice);
            ViewBag.TotalSales = TotalSales;

            decimal TotalExpense = OrderItemData.Sum(a => a.Items.CostToProduce * a.Quantity);
            ViewBag.TotalExpense = TotalExpense;


            decimal Profit = TotalSales - TotalExpense;
            ViewBag.Profit = Profit;




            return View();


        }
    }
}
