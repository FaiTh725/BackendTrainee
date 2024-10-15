using Microsoft.AspNetCore.Mvc;
using Trainee.Models.Client;
using Trainee.Services.Interfaces;

namespace Trainee.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService clientService;

        public ClientController(IClientService clientService)
        {
            this.clientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClients()
        {

            var result = await clientService.GetAllClients();

            return new JsonResult(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetClient(int id)
        {
            var result = await clientService.GetClient(id);

            return new JsonResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient(CreateClient client)
        {
            var result = await clientService.CreateClient(client);

            return new JsonResult(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var result = await clientService.DeleteClient(id);

            return new JsonResult(result);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateClient(UpdateClient client)
        {
            var result = await clientService.UpdateClient(client);

            return new JsonResult(result);
        }
    }
}
