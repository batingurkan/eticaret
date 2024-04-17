using eTicaretDAL;
using eTicaretEL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace eTicaret.Controllers.User
{
    public class OrderController : Controller
    {
        private readonly DataContext _dataContext;

        public OrderController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(string cargoCompany, string cargoNo)
        {
            var userId = HttpContext.Session.GetInt32("userId");

            var cartItems = await _dataContext.CartItem
                .Include(ci => ci.Product)
                .Where(ci => ci.UserId == userId)
                .ToListAsync();

            if (cartItems == null || !cartItems.Any())
            {
                return BadRequest("Sepetiniz boş!");
            }

            // Sepet tutarını hesapla
            decimal totalAmount = cartItems.Sum(ci => ci.Product.Price);

            // Sipariş oluştur
            var order = new Order
            {
                UserId = userId.Value,
                OrderDate = DateTime.Now,
                OrderStatusId = 1,
                CargoId = 1,
                CargoNo = cargoNo,
                TotalAmount = totalAmount,
                OrderDetail = cartItems.Select(ci => new OrderDetail
                {
                    ProductId = ci.ProductId,
                    Price = ci.Product.Price
                }).ToList()
            };

            _dataContext.Order.Add(order);
            await _dataContext.SaveChangesAsync();

            // Sepetten ürünleri kaldır
            _dataContext.CartItem.RemoveRange(cartItems);
            await _dataContext.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
        
    }
}
