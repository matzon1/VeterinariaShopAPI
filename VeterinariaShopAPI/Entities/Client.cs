using Microsoft.AspNetCore.Identity;

namespace VeterinariaShopAPI.Entities
{
    public class Client : User
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Name { get => FirstName + " " + LastName; }

        public Client(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
