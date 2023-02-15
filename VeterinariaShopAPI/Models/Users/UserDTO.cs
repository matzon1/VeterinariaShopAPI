using VeterinariaShopAPI.Models;

namespace VeterinariaShopAPI.Models
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRoleDTO UserRole { get; set; }
    }
}
