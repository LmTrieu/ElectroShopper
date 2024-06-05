using Microsoft.AspNetCore.Mvc;
using RookieEShopper.Application.Repositories;
using RookieEShopper.Domain.Data.Entities;

namespace RookieEShopper.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpPost]
        public ActionResult Create(Order order)
        {
            _orderRepository.CreateOrderAsync(order);
            return View();
        }
    }
}