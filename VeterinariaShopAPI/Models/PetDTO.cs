namespace VeterinariaShopAPI.Models
{
    public class PetDTO
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public float Weight { get; set; }
        public int Castrated { get; set; }
    }
}
