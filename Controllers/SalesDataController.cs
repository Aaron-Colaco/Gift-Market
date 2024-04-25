
using AaronColacoAsp.NETProject.Data;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")]
       public IActionResult DatePicker()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult SalesDashboard(DateTime Date1, DateTime Date2)
        {

            var OrderData = _context.Order.Where(a => a.OrderTime >= Date1 && a.OrderTime <= Date2);
            var GiftData = _context.Gift.Where(a => a.Order.OrderTime >= Date1 && a.Order.OrderTime <= Date2); ;
            var OrderItemData = _context.OrderItem.Where(a => a.Orders.OrderTime >= Date1 && a.Orders.OrderTime <= Date2); ;

           
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

            decimal AverageOrderCost;
            if (TotalProductsSold == 0)
            {
                AverageOrderCost = 0;
            }
            else
            {
                AverageOrderCost = OrderData.Average(a => a.TotalPrice);
            }

            ViewBag.AverageOrderCost = AverageOrderCost;

            return View();


        }
    }
}
