namespace VeterinariaShopAPI.Models
{
    public class LogInDTO
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Id { get; set; }

        public LogInDTO(string name, string role, string id)
        {
            Name = name;
            Role = role;
            Id = id;
        }
    }
}
