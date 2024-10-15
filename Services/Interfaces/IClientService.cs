using Trainee.Domain.Responses;
using Trainee.Models.Client;

namespace Trainee.Services.Interfaces
{
    public interface IClientService
    {
        public Task<Response> GetAllClients();

        public Task<Response> GetClient(int id);

        public Task<Response> CreateClient(CreateClient createClient);

        public Task<Response> UpdateClient(UpdateClient updateClient);

        public Task<Response> DeleteClient(int id);
    }
}
