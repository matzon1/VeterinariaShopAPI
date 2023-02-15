
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VeterinariaShopAPI.Entities;

namespace VeterinariaShopAPI.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<Client> _userManagerClient;

        public ClientRepository(DataContext context, IHttpContextAccessor httpContextAccessor, UserManager<Client> userManagerStudent)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _userManagerClient = userManagerStudent;
        }

        public IEnumerable<Client> GetClients()
        {
            return _context.Clients
                .ToList();
        }

    }
}
