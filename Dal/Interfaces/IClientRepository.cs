using Trainee.Domain.Entities;
using Trainee.Models.Client;

namespace Trainee.Dal.Interfaces
{
    public interface IClientRepository
    {
        public Task<Client> CreateClient(Client client);

        public Task<List<ClientWithTasks>> GetClients();

        public Task<Client> GetClientById(int id);

        public Task<Client> UpdateClient(Client client);

        public Task<bool> DeleteClient(int id);
    }
}
