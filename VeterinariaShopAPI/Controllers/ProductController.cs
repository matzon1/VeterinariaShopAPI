using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VeterinariaShopAPI.Models;

namespace VeterinariaShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {


        private static List<ProductDTO> products = new List<ProductDTO>
            {
                new ProductDTO
                {
                    Id = 1,
                    Name = "Combo Perro",
                    Food = 0.8,
                    SupplementAge = 0,
                    SupplementCastrated = 0
                },
                new ProductDTO
            {
                Id = 2,
                Name = "Combo Gato",
                Food = 0.5,
                SupplementAge = 0,
                SupplementCastrated = 0
            }
};

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(products);
        }
    }
}
