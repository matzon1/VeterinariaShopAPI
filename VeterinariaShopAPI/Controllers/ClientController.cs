
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VeterinariaShopAPI.Models.Users;
using VeterinariaShopAPI.Services;

namespace VeterinariaShopAPI.Controllers
{
    [Route("api/client")]
    [ApiController]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IClientServices _clientServices;
        public StudentController(IClientServices clientServices)
        {
            _clientServices = clientServices;
        }
        [HttpGet]
        public ActionResult<IEnumerable<ClientDTO>> GetClients()
        {
            var client = _clientServices.GetClients();
            return Ok(client);
        }


        [HttpPost]
        [AllowAnonymous]
        public IActionResult ClientsSignUp(ClientToCreateDTO client)
        {
            try
            {
                var newClient = _clientServices.ClientsSignUp(client);
                if (newClient is null)
                    return BadRequest();
                return Created("/api/student", newClient);
            }
            catch
            {
                return BadRequest();
            }

        }


    }
}
