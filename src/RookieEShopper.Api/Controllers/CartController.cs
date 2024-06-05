using Microsoft.AspNetCore.Mvc;
using RookieEShopper.Application.Repositories;

namespace RookieEShopper.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
    }
}