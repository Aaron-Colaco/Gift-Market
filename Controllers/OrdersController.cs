using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AaronColacoAsp.NETProject.Data;
using AaronColacoAsp.NETProject.Models;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Security.Claims;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Configuration;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stripe;

namespace AaronColacoAsp.NETProject.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SearchByCustomer(string CustomerName)
      {
        var Results = _context.Order.Where(a => a.Customers.FullName.Contains(CustomerName) || a.Customers.Email.Contains(CustomerName) && a.StatusId != 1).Include(a => a.Customers).Include(a => a.Status);
        return View("Index", await Results.ToListAsync());
       }
        [Authorize(Roles = "Admin")]
        public async Task< IActionResult> FilterOrdersByDate(DateTime Date1, DateTime Date2)
        {

            var OrderData = _context.Order.Where(a => a.OrderTime >= Date1 && a.OrderTime <= Date2 && a.StatusId != 1).Include(a => a.Status).Include(a => a.Customers);
            return View("Index", await OrderData.ToListAsync());
        }

            // GET: Orders
            public async Task<IActionResult> Index(string sortOrder, int Page = 1)
        {

            var OrderData = from a in _context.Order
                            select a;

            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";


            switch (sortOrder)
            {
              
                case "Date":
                    OrderData = OrderData.OrderBy(a => a.OrderTime);
                    break;
                case "date_desc":
                    OrderData = OrderData.OrderByDescending(s => s.OrderTime);
                    break;
                default:
                    OrderData = OrderData.OrderByDescending(s => s.OrderTime);
                    break;
            }

            if (User.IsInRole("Admin"))
            {
                OrderData = OrderData.Where(a => a.StatusId != 1).Include(o => o.Status).Include(a => a.Customers);
          
            }
            else
            {
                OrderData = OrderData.Include(o => o.Status).Include(a => a.Customers).Where(a => a.CustomerId == User.FindFirstValue(ClaimTypes.NameIdentifier));
           
            }


            const int ItemsPerPage = 20;
            ViewBag.Pages = (int)Math.Ceiling((double)OrderData.Count() / ItemsPerPage);
            ViewBag.PageNumber = Page;

            return View(await OrderData.Skip((Page - 1) * ItemsPerPage).Take(20).AsNoTracking().ToListAsync());








        
            














        }

        public IActionResult CheckOut(string id)
        {

            ViewBag.OrderId = id;
            return View(CheckOut);

        }
        // GET: Orders/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Status)
                .Include(a => a.Customers)
                .Include(a => a.Gifts).ThenInclude(a => a.giftRecipient)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> StatusUpdate(string id)
        {
            ViewBag.OrderId = id;
            var Order = _context.Order.Where(a => a.OrderId == id).Include(a => a.Customers).First();
            ViewBag.Customer = Order.Customers.FullName;
            ViewBag.Date = Order.OrderTime;
            ViewBag.Status = _context.Status;

            return View("StatusUpdate");
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrder(int Status, string OrderId)
        {
            var order = _context.Order.Where(a => a.OrderId == OrderId).FirstOrDefault();
            order.StatusId = Status;
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));

        }


        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Customers)
                .Include(o => o.Status)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                _context.Order.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(string id)
        {
            return _context.Order.Any(e => e.OrderId == id);
        }
    }
}
