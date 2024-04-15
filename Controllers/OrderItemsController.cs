using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AaronColacoAsp.NETProject.Data;
using AaronColacoAsp.NETProject.Models;
using System.Security.Claims;
using NuGet.Protocol.Core.Types;
using Microsoft.AspNetCore.Authorization;

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
            string OrderId = await CheckUserOrders();
            var OrderItem = new OrderItem
            {         
                OrderId = OrderId,
                ItemId = ItemId,
                Quantity = 1
            };
          
            _context.OrderItem.Add(OrderItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new {id = OrderId});
        }

        [HttpPost]
        [Authorize]
        public async Task<string> CheckUserOrders()
        {

            var Customer = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var UserOrder = _context.Order.Where(a => a.CustomerId == Customer).FirstOrDefault();


            if (UserOrder == null)
            {

                    var NewOrder = new Order
                    {
                        CustomerId = Customer,
                        StatusId = 1,
                        OrderTime = DateTime.Now,
                    };

                    _context.Order.Add(NewOrder);
                    await _context.SaveChangesAsync();

                    return NewOrder.OrderId;
            }
            else
            {
                return (UserOrder.OrderId);
            }
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
