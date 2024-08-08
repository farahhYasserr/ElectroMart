using E_Commerce.DataAccess.Implementation;
using E_Commerce.Entities.Models;
using E_Commerce.Entities.Repositories;
using E_Commerce.Entities.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;

namespace E_Commerce.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicatonUser> _userManger;
        public CartController(IUnitOfWork unitOfWork, UserManager<ApplicatonUser> userManger)
        {
            _unitOfWork = unitOfWork;
            _userManger = userManger;
        }
        public IActionResult EmptyCart()
        {
            return View();
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManger.GetUserAsync(User);
            IEnumerable<ShoppingCart> ShoopingsCarts = _unitOfWork.ShoppingCarts.GetAll(u =>
            u.UserId == user.Id &&
            u.OrderId == null,
            Includeword: "Product"
            );
            if (ShoopingsCarts.Count() < 1)
            {
                return View("EmptyCart");

            }
            return View(new ShoppingCartsVM
            {
                shoppingCarts = ShoopingsCarts,
                ToatalPrice = GetTotalPrice(ShoopingsCarts),
                TotalItems = (int)ShoopingsCarts.Count()
            });
        }
        private Decimal GetTotalPrice(IEnumerable<ShoppingCart> shoppingCarts)
        {
            Decimal TotalPrice = 0;
            foreach (var item in shoppingCarts)
            {
                TotalPrice += (item.Product.Price * item.Count);
            }
            return TotalPrice;
        }

        [HttpPost]
        public async Task<IActionResult> ChangeCount(int id, [FromBody] dynamic data)
        {
            int newCount = data.count;
            var user = await _userManger.GetUserAsync(User);

            ShoppingCart shoppingCart = _unitOfWork.ShoppingCarts.GetFirstorDefault(u => u.UserId == user.Id && u.ProductId == id);

            shoppingCart.Count = newCount;
            _unitOfWork.ShoppingCarts.Update(shoppingCart);
            _unitOfWork.Save();

            IEnumerable<ShoppingCart> ShoopingsCarts = _unitOfWork.ShoppingCarts.GetAll(u =>
            u.UserId == user.Id,
            Includeword: "Product"
            );

            decimal totalPrice = GetTotalPrice(ShoopingsCarts);

            return Ok(new { totalPrice });
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveItem(int id)
        {

            var user = await _userManger.GetUserAsync(User);

            ShoppingCart shoppingCart = _unitOfWork.ShoppingCarts.GetFirstorDefault(u => u.UserId == user.Id && u.ProductId == id);


            _unitOfWork.ShoppingCarts.Remove(shoppingCart);
            _unitOfWork.Save();

            IEnumerable<ShoppingCart> ShoopingsCarts = _unitOfWork.ShoppingCarts.GetAll(u =>
            u.UserId == user.Id,
            Includeword: "Product"
            );
            var count = ShoopingsCarts.Count();
            HttpContext.Session.SetInt32("ShoppingCount" + user.Id, count);

            decimal totalPrice = GetTotalPrice(ShoopingsCarts);

            return Ok(new { totalPrice });
        }

        public async Task<IActionResult> Summary()
        {
            var user = await _userManger.GetUserAsync(User);
            IEnumerable<ShoppingCart> ShoopingsCarts = _unitOfWork.ShoppingCarts.GetAll(u =>
                  u.UserId == user.Id &&
                  u.OrderId == null,
                 Includeword: "Product"
            );
            Order order = new Order
            {
                User = user,
                TotalPrice = GetTotalPrice(ShoopingsCarts),
                UserId = user.Id,
                ShoppingCarts = (List<ShoppingCart>)ShoopingsCarts,
            };
            return View(order);
        }
        [HttpPost]
        public async Task<IActionResult> Summary(Order orderModel)
        {
            var user = await _userManger.GetUserAsync(User);
            IEnumerable<ShoppingCart> ShoopingsCarts = _unitOfWork.ShoppingCarts.GetAll(u =>
                  u.UserId == user.Id &&
                  u.OrderId == null,
                 Includeword: "Product"
            );

            Order order = new Order
            {
                User = user,
                TotalPrice = GetTotalPrice(ShoopingsCarts),
                UserId = user.Id,
                ShoppingCarts = (List<ShoppingCart>)ShoopingsCarts,
                OrderStatus = "Pendding"
            };
            _unitOfWork.Orders.Add(order);
            user.Orders.Add(order);
            _unitOfWork.Users.Update(user);
            _unitOfWork.Save();

            var domain = "https://localhost:7010/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"Customer/Cart/OrderConfirmation?id={order.Id}",
                CancelUrl = domain + $"Customer/Cart/Index",
            };

            foreach (var item in ShoopingsCarts)
            {
                var session2 = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)item.Product.Price * 100,
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                        },
                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(session2);
            }

            var service = new SessionService();
            Session session = service.Create(options);
            order.SessionId = session.Id;
            // order.PaymentIntentId = session.PaymentIntentId;
            _unitOfWork.Orders.Update(order);
            _unitOfWork.Save();

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);


        }

        public IActionResult OrderConfirmation(int id)
        {
            Order order = _unitOfWork.Orders.GetFirstorDefault(u => u.Id == id);
            var service = new SessionService();
            Session session = service.Get(order.SessionId);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                order.OrderStatus = "Approved";
                _unitOfWork.Orders.Update(order);
                order.PaymentIntentId = session.PaymentIntentId;
                _unitOfWork.Save();
            }
            HttpContext.Session.Clear();
            return View(id);
        }

    }
}
