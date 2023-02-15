
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeterinariaShopAPI.Entities
{
    public class User : IdentityUser<Guid>
    {
    }
}
