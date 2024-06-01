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


        public async Task<IActionResult> OpenCart()
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



        [Authorize]
        public async Task<IActionResult> AddToCart(int ItemId)
        {
            if (!User.Identity.IsAuthenticated)
            {
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



            if (ExistingItem != null && ExistingItem.Quantity >= 8)
            {
                ViewBag.MaxQuantiy = true;
                return RedirectToAction("Index", new { id = OrderId, MaxQuantity = true });

            }

            else if (ExistingItem != null)
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

        public async Task<IActionResult> Index(bool CartFull = false, bool MaxQuantity = false)
        {


            string id = await CheckUserOrders();
            var Order = _context.Order.Where(a => a.OrderId == id).FirstOrDefault();


            ViewBag.CartFull = CartFull;
            ViewBag.MaxQuantity = MaxQuantity;
            ViewBag.StatusId = Order.StatusId;
            ViewBag.TotalRrice = Order.TotalPrice;

            var OrderItems = await GetOrder();


            return View(GetOrder);
        }

        public async Task<IActionResult> ProcessOrder(string FullName, string PhoneNumber, string BoxColour, string RibbionColour, String GiftMessage, string RecipientPhone, string RecipientName, string DeliveryAddress, string City, int PostalCode)
        {

            string OrderId = await CheckUserOrders();
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

  
        public IActionResult Payment(string OrderId)
        {
            StripeConfiguration.ApiKey = _stripeSettings.SecretKey;

            var ItemsInOrder = _context.OrderItem.Where(a => a.OrderId == OrderId).Include(a => a.Items);


            var Options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                CustomerEmail = User.Identity.Name,
                SuccessUrl = "https://Localhost:7002/Home",
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
