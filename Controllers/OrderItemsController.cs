using AaronColacoAsp.NETProject.Data;
using AaronColacoAsp.NETProject.Models;
using AaronColacoAsp.NETProject.Stripe_Payment_API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;
using SessionCreateOptions = Stripe.Checkout.SessionCreateOptions;

namespace AaronColacoAsp.NETProject.Controllers
{
    public class OrderItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly StripeSettings _stripeSettings;


        public OrderItemsController(IOptions<StripeSettings> stripeSettings, ApplicationDbContext context)
        {
            _stripeSettings = stripeSettings.Value;
            _context = context;


        }







        [Authorize]
        public async Task<IActionResult> AddToCart(int ItemId)
        {
            string OrderId = await CheckUserOrders();


            var ItemsInOrder = _context.OrderItem.Where(a => a.OrderId == OrderId).Include(a => a.Items);


            var ExistingItem = ItemsInOrder.Where(a => a.ItemId == ItemId).FirstOrDefault();

            if (ExistingItem != null)
            {
                ExistingItem.Quantity++;
          
            }
            else
            {

                var OrderItem = new OrderItem
                {
                    OrderId = OrderId,
                    ItemId = ItemId,
                    Quantity = 1
                };

                _context.OrderItem.Add(OrderItem);
            }

            await _context.SaveChangesAsync();


            return RedirectToAction("Index", new { id = OrderId });
        }

        [HttpPost]
        [Authorize]
        public async Task<string> CheckUserOrders()
        {

            var Customer = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var UserOrder = _context.Order.Where(a => a.CustomerId == Customer && a.StatusId == 1).FirstOrDefault();


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

            var OrderItems = _context.OrderItem.Include(o => o.Items).Where(a => a.OrderId == id).Include(o => o.Orders);
            



            return View(await OrderItems.ToListAsync());
        }

        public async Task<IActionResult> ProcessOrder(string OrderId, string FullName, string PhoneNumber, string BoxColour, string RibbionColour, String GiftMessage, string RecipientPhone, string RecipientName, string DeliveryAddress, string City, int PostalCode)
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
            
           


            return RedirectToAction("Payment", new{ OrderId = OrderId });


        }





        public IActionResult Payment(string OrderId)
        {
         StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
            
            var ItemsInOrder = _context.OrderItem.Where(a => a.OrderId == OrderId).Include(a => a.Items);
           

            var Options = new SessionCreateOptions
            {  
                LineItems = new List<SessionLineItemOptions>(),
                CustomerEmail = User.Identity.Name, SuccessUrl ="https://Localhost:7002/Home",Mode = "payment", ClientReferenceId = User.FindFirstValue(ClaimTypes.NameIdentifier),           
             };

            foreach (var item in ItemsInOrder)
            {
                var OrderedItem = new SessionLineItemOptions()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)item.Items.Price * 100, Currency = "nzd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Items.Name
                        }
                    },
                    Quantity = item.Quantity
                };
                Options.LineItems.Add(OrderedItem);
                
            }

            var service = new SessionService();
            var session = service.Create(Options);


            return Redirect(session.Url);




        }
    }
    }
