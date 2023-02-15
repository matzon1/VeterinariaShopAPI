using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using VeterinariaShopAPI.Entities;
using VeterinariaShopAPI.Models.Users;
using VeterinariaShopAPI.Repository;
using VeterinariaShopAPI.Services;

namespace VeterinariaShopAPI.Services
{
    public class ClientServices : IClientServices
    {
        private readonly UserManager<User> _userManager;
        private readonly UserManager<Client> _userManagerStudent;
        private readonly UserManager<Admin> _userManagerAdmin;
        private readonly IClientRepository _clientRepository;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public ClientServices(IMapper mapper,
            UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor,
            IClientRepository clientRepository,
            UserManager<Client> userManagerStudent,
            UserManager<Admin> userManagerAdmin,
            DataContext context
            )
        {
            _userManager = userManager;
            _userManagerStudent = userManagerStudent;
            _userManagerAdmin = userManagerAdmin;
            _clientRepository = clientRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _context = context;
        }
        public IEnumerable<ClientDTO> GetClients()
        {
            return _mapper.Map<IEnumerable<ClientDTO>>(_clientRepository.GetClients());
        }

     
        public ClientDTO? ClientsSignUp(ClientToCreateDTO client)
        {
            Client clientToCreate = new Client(client.FirstName, client.LastName)
            {
                Email = client.Email,
                UserName = client.Email,
            };
            var newUser =  _userManager.CreateAsync(clientToCreate, client.Password).Result;
            var addRole = _userManager.AddToRoleAsync(clientToCreate, "Cliente").Result;
            if (newUser.Succeeded && addRole.Succeeded)
            {

                var clientCreated =  _userManager.FindByIdAsync(clientToCreate.Id.ToString()).Result;
                return _mapper.Map<ClientDTO>(clientCreated);
            }
            return null;
        }


    }
}
