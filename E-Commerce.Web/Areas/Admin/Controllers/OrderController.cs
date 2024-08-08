using E_Commerce.DataAccess.Data;
using E_Commerce.Entities.Models;
using E_Commerce.Entities.Repositories;
using E_Commerce.Entities.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace E_Commerce.Web.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class OrderController : Controller
	{
		IUnitOfWork _unitOfWork;

		public OrderController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public IActionResult Index()
		{

			return View();
		}
		[HttpGet]
		public IActionResult GetAllOrders()
		{
			var Orders = _unitOfWork.Orders.GetAll(Includeword: "User");
			return Json(Orders);
		}

		public IActionResult OrderDetails(int OrderId)
		{
			var Order = _unitOfWork.Orders.GetFirstorDefault(o => o.Id == OrderId, Includeword: "User,ShoppingCarts");
			List<ShoppingCart> TempList = new List<ShoppingCart>();
			foreach (var item in Order.ShoppingCarts)
			{
				var shop = _unitOfWork.ShoppingCarts.GetFirstorDefault(u => u.ProductId == item.ProductId && u.OrderId == OrderId, Includeword: "Product");
				TempList.Add(shop);
			}
			Order.ShoppingCarts = TempList;
			return View(Order);
		}

		[HttpPost]
		public IActionResult UpdateOrderDetails(Order order)
		{
			_unitOfWork.Orders.Update(order);
			_unitOfWork.Save();
			Console.WriteLine("odsk");
			return Ok();
		}
		/*[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult StartProccess()
		{
			_unitOfWork.OrderHeader.UpdateStatus(OrderVM.OrderHeader.Id, SD.Proccessing, null);
			_unitOfWork.Complete();
			
			TempData["Update"] = "Order Status has Updated Successfully";
			return RedirectToAction("Details", "Order", new { orderid = OrderVM.OrderHeader.Id });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult StartShip()
		{
			var order = _unitOfWork.Orders.GetFirstorDefault(u => u.Id == );
			order.TrakcingNumber = order.TrakcingNumber;
			order.Carrier = order.Carrier;
			order.OrderStatus = "Shipped";
			order.ShippingDate = DateTime.Now;

			_unitOfWork.Orders.Update(order);
			_unitOfWork.Save();

			TempData["Update"] = "Order has Shipped Successfully";
			return RedirectToAction("Details", "Order", new { orderid = order.Id });
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult CancelOrder()
		{
			var orderfromdb = _unitOfWork.Order.GetFirstorDefault(u => u.Id == OrderVM.Orders.Id);
			if (orderfromdb.PaymentStatus == SD.Approve)
			{
				var option = new RefundCreateOptions
				{
					Reason = RefundReasons.RequestedByCustomer,
					PaymentIntent = orderfromdb.PaymentIntentId
				};

				var service = new RefundService();
				Refund refund = service.Create(option);

				_unitOfWork.Orders.UpdateStatus(orderfromdb.Id, SD.Cancelled, SD.Refund);
			}
			else
			{
				_unitOfWork.Orders.Update(order);
			}
			_unitOfWork.Save();

			TempData["Update"] = "Order has Cancelled Successfully";
			return RedirectToAction("Details", "Order", new { orderid = OrderVM.OrderHeader.Id });
		}


*/
	}
}
