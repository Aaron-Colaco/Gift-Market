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

namespace AaronColacoAsp.NETProject.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> SearchByCustomer(string CustomerName)
        {
            var Results = _context.Order.Where(a => a.Customers.FullName.Contains(CustomerName) || a.Customers.Email.Contains(CustomerName) && a.StatusId != 1).Include(a => a.Customers).Include(a => a.Status);
            return View("Index", await Results.ToListAsync());
        }
        public async Task< IActionResult> FilterOrdersByDate(DateTime Date1, DateTime Date2)
        {

            var OrderData = _context.Order.Where(a => a.OrderTime >= Date1 && a.OrderTime <= Date2 && a.StatusId != 1).Include(a => a.Status).Include(a => a.Customers); 
            return View("Index", await OrderData.ToListAsync());
        }

            // GET: Orders
            public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Order.Include(o => o.Status);
            return View(await applicationDbContext.ToListAsync());
        }

        public IActionResult CheckOut(string id)
        {

            ViewBag.OrderId = id;
            return View(CheckOut);

        }

        public async Task<IActionResult> ProcessOrder(string OrderId, string FullName, string PhoneNumber, string BoxColour, string RibbionColour,String GiftMessage, string RecipientPhone,string RecipientName, string DeliveryAddress, string City, int PostalCode)
        {

            try
            {


                var OrderToProcess = _context.Order.Where(a => a.OrderId.Equals(OrderId)).First();
                var Customer = _context.Customer.Where(a => a.Id.Equals(OrderToProcess.CustomerId)).First();


                Customer.FullName = FullName;
                Customer.PhoneNumber = PhoneNumber;
                OrderToProcess.StatusId = 2;
                OrderToProcess.OrderTime = DateTime.Now;
                OrderToProcess.DeliveryAddress = DeliveryAddress;
                OrderToProcess.City = City;
                OrderToProcess.PostalCode = PostalCode;


                var Gift = new Gift
                {
                    GiftId = OrderId,
                    OrderId = OrderId,
                    BoxColour = BoxColour,
                    RibbonColour = RibbionColour,
                    Message = GiftMessage
                };

                _context.Gift.Add(Gift);

                await _context.SaveChangesAsync();

                var GiftRecipient = new GiftRecipient
                {
                    RecipientId = OrderId,
                    GiftId = Gift.GiftId,
                    Name = RecipientName,
                    PhoneNumber = RecipientPhone

                };
                _context.GiftRecipient.Add(GiftRecipient);

                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException) {

                return RedirectToAction("Error");
            }
            

            return RedirectToAction("Index");


        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(string id)
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

]        // GET: Orders/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Id");
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "StatusId");
            return View();
        }

        
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderTime,TotalPrice,StatusId")] Order order)
        {
            if (!ModelState.IsValid)
            {
                order.CustomerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Id", order.CustomerId);
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "StatusId", order.StatusId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Id", order.CustomerId);
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "StatusId", order.StatusId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("OrderId,OrderTime,TotalPrice,StatusId,CustomerId")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "Id", "Id", order.CustomerId);
            ViewData["StatusId"] = new SelectList(_context.Status, "StatusId", "StatusId", order.StatusId);
            return View(order);
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
