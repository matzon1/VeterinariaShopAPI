using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VeterinariaShopAPI.Models;

namespace VeterinariaShopAPI.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly DataContext _context;

        public AdminController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> Get()
        {
            return Ok(await _context.Admins.ToListAsync());
        }

    }
}
