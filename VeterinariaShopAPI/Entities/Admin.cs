using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace VeterinariaShopAPI.Entities

{
    public class Admin : User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get => FirstName + " " + LastName; }
        public Admin(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
