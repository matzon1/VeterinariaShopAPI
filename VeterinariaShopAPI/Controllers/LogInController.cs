
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VeterinariaShopAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/login")]
    public class LogInController : ControllerBase
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public LogInController(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        [HttpGet]
        public ActionResult<string> GetUser()
        {
            var user = _contextAccessor.HttpContext.User;
            
            var response = new
            {
                Claims = user.Claims.Select(u => new
                {
                    u.Type,
                    u.Value,
                }).ToList(),
                user.Identity.IsAuthenticated,
                user.Identity.AuthenticationType
            };

            return Ok(response);
        }


    }
}
