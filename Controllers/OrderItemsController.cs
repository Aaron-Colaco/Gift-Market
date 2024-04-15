using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AaronColacoAsp.NETProject.Data;
using AaronColacoAsp.NETProject.Models;

namespace AaronColacoAsp.NETProject.Controllers
{
    public class OrderItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> AddToCart(int ItemId)
        {
            var OrderItem = new OrderItem
            {         
                OrderId = "1a",
                ItemId = ItemId,
                Quantity = 1
            };
            String OrderId = "1a";
            _context.OrderItem.Add(OrderItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new {id = OrderId});
        }
        
        public async Task<IActionResult> Index(string id)
        {
            ViewBag.OrderId = id;
            var Order = _context.Order.Where(a => a.OrderId == id).First();
            ViewBag.StatusId = Order.StatusId;
            var applicationDbContext = _context.OrderItem.Include(o => o.Items).Where(a => a.OrderId == id).Include(o => o.Orders);
            return View(await applicationDbContext.ToListAsync());
        }

     
        
        private bool OrderItemExists(string id)
        {
            return _context.OrderItem.Any(e => e.OrderItemId == id);
        }
    }
}
