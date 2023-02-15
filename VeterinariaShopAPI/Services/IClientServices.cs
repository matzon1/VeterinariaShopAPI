

using VeterinariaShopAPI.Models.Users;

namespace VeterinariaShopAPI.Services
{
    public interface IClientServices
    {
        IEnumerable<ClientDTO> GetClients();
        ClientDTO? ClientsSignUp(ClientToCreateDTO client);
        
    }
}
