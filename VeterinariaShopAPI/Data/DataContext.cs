using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VeterinariaShopAPI.Entities;
using VeterinariaShopAPI.Models;
using VeterinariaShopAPI.Models.Users;

namespace VeterinariaShopAPI.Data
{
    public class DataContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<PetDTO> Pets { get; set; }

        public DbSet<Admin> Admins => Set<Admin>();

        public DbSet<Client> Clients => Set<Client>();
    }
}