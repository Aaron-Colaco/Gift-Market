using AaronColacoAsp.NETProject.Data;
using AaronColacoAsp.NETProject.Models;
using AaronColacoAsp.NETProject.Stripe_Payment_API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using Stripe.Climate;
using System.Security.Claims;
using static System.Net.WebRequestMethods;
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


        public async Task<IActionResult> OpenCart(string order)
        {
            var Order = await GetOrder();
            return View("Index", Order);





        }


        public async Task<IActionResult> Delete(int ItemId)
        {

            var OrderItems = await GetOrder();

            var OrderItemToRemove = OrderItems.Where(a => a.ItemId == ItemId).FirstOrDefault();

            _context.OrderItem.Remove(OrderItemToRemove);
            _context.SaveChanges();

            return RedirectToAction("OpenCart");
        }



        public async Task<IActionResult> AddToCart(int ItemId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                //to the hosted site after 60 day trial will brake
                return RedirectPermanent("/Identity/Account/Register");
            }





            string OrderId = await CheckUserOrders();


            var ItemsInOrder = await GetOrder();


            if (ItemsInOrder.Sum(a => a.Quantity) >= 35)
            {
                ViewBag.CartFull = 1;

                return RedirectToAction("Index", new { id = OrderId, CartFull = true });
            }



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


            ItemsInOrder = await GetOrder();

            var Order = _context.Order.Where(a => a.OrderId == OrderId).First();

            Order.TotalPrice = ItemsInOrder.Sum(a => a.Items.Price * a.Quantity);


            await _context.SaveChangesAsync();


            return RedirectToAction("Index", "Items", new { displayPopUp = true, item = ItemId });
        }



        [HttpPost]
        [Authorize]
        public async Task<string> CheckUserOrders()
        {

            var Customer = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var UserOrder = _context.Order.Where(a => a.CustomerId == Customer && a.StatusId == 1).FirstOrDefault();


            if (UserOrder == null)
            {

                var NewOrder = new Models.Order
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

        public async Task<IActionResult> Index(string id, bool CartFull = false, bool MaxQuantity = false)
        {


            var Order = _context.Order.Where(a => a.OrderId == id).FirstOrDefault();

            if (Order.CustomerId != User.FindFirstValue(ClaimTypes.NameIdentifier) && !User.IsInRole("Admin"))
            {

                return View("Cant find Order that Belongs to you");

            }



            ViewBag.CartFull = CartFull;
            ViewBag.MaxQuantity = MaxQuantity;
            ViewBag.StatusId = Order.StatusId;
            ViewBag.TotalRrice = Order.TotalPrice;

            var OrderItem = await _context.OrderItem.Where(a => a.OrderId == Order.OrderId).Include(a => a.Items).ToListAsync();


            return View(OrderItem);
        }

        public async Task<IActionResult> ProcessOrder(string FullName, string PhoneNumber, string BoxColour, string RibbionColour, String GiftMessage, string RecipientPhone, string RecipientName, string DeliveryAddress, string City, int PostalCode)
        {

            string OrderId = await CheckUserOrders();
            var OrderToProcess = _context.Order.Where(a => a.OrderId.Equals(OrderId)).First();
            var Customer = _context.Customer.Where(a => a.Id.Equals(OrderToProcess.CustomerId)).First();


            Customer.FullName = FullName;
            Customer.PhoneNumber = PhoneNumber;
           
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

            return RedirectToAction("Payment", new { OrderId = OrderId });


        }


        public async Task<List<OrderItem>> GetOrder()
        {
            string orderId = await CheckUserOrders();

            var listOrderItems = await _context.OrderItem
                .Include(o => o.Items)
                .Include(o => o.Orders)
                .Where(a => a.OrderId == orderId)
                .ToListAsync();

            return listOrderItems;
        }

      
         
       
        public async Task<IActionResult>Success()
        {

            string OrderId = await CheckUserOrders();
            var UserOrder = _context.Order.Where(a => a.OrderId == OrderId).Include(a => a.Customers).FirstOrDefault();

            UserOrder.StatusId = 2;
            UserOrder.OrderTime = DateTime.Now;

  

            string EmailBody = "<h1>Dear " + UserOrder.Customers.FullName + ",</h1><p>Thank you for your order here at Kiwi Gift Market. We will work on processing your order as soon as we can.</p><p>Your total cost was $" + UserOrder.TotalPrice.ToString() + ".</p>";

            HomeController.SendEmailToCusotmer(UserOrder.Customers.Email, EmailBody, "Thanks " + UserOrder.Customers.FullName);

            _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }


     
      public async Task Cancel()
        {
            string OrderId = await CheckUserOrders();
            var UserOrder = _context.Order.Where(a => a.OrderId == OrderId).First();

            _context.Remove(UserOrder);
            _context.SaveChanges();
        }



        public IActionResult Payment(string OrderId)
        {
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;

            var ItemsInOrder = _context.OrderItem.Where(a => a.OrderId == OrderId).Include(a => a.Items);


            var Options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                CustomerEmail = User.Identity.Name,
                SuccessUrl = "https://localhost:7002/OrderItems/Success",
                CancelUrl = "https://localhost:7002/OrderItems/Cancel",
                Mode = "payment",
                ClientReferenceId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            };
 

            
            foreach (var item in ItemsInOrder)
            {
                var OrderedItem = new SessionLineItemOptions()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)item.Items.Price * 100,
                        Currency = "nzd",
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
