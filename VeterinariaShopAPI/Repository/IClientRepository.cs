

using VeterinariaShopAPI.Entities;

namespace VeterinariaShopAPI.Repository
{
    public interface IClientRepository
    {
        public IEnumerable<Client> GetClients();
    }
}
