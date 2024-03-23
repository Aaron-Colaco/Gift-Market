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
            var GiftData = _context.Gift;

            var OrderItemData = _context.OrderItem;

            decimal TotalSales = OrderItemData.Sum(a => a.Items.Price * a.Quantity);
            ViewBag.TotalSales = TotalSales;

            decimal TotalExpense = OrderItemData.Sum(a => a.Items.CostToProduce * a.Quantity);
            ViewBag.TotalExpense = TotalExpense;


            decimal Profit = TotalSales - TotalExpense;
            ViewBag.Profit = Profit;

            int TotalGifts = GiftData.Count();
            ViewBag.TotalGifts = TotalGifts;

            var TotalProductsSold = OrderItemData.Sum(a => a.Quantity);
            ViewBag.TotalProductsSold = TotalProductsSold;

            var AverageOrderCost = OrderData.Average(a => a.TotalPrice);
            ViewBag.AverageOrderCost = AverageOrderCost;



            return View();


        }
    }
}
