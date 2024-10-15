using Serilog;
using Trainee.Dal.Interfaces;
using Trainee.Domain.Entities;
using Trainee.Domain.Enums;
using Trainee.Domain.Responses;
using Trainee.Models.Client;
using Trainee.Services.Interfaces;

namespace Trainee.Services.Implementations
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            this.clientRepository = clientRepository;
        }

        public async Task<Response> CreateClient(CreateClient createClient)
        {
            try
            {
                var client = await clientRepository.CreateClient(new Client 
                { 
                    Name = createClient.Name,
                    Email = createClient.Email, 
                    Phone = createClient.Phone
                });

                Log.Debug($"Create client with id {client.Id}");

                return new DataResponse<GetClient>
                {
                    Description = "Create new client success",
                    StatusCode = StatusCode.Ok,
                    Data = new GetClient 
                    {
                        Id = client.Id,
                        Name = createClient.Name,
                        Email = createClient.Email,
                        Phone = createClient.Phone,
                        CreateAt = client.CreatedAt,
                        UpdateAt = client.UpdatedAt,
                    }
                };
            }
            catch (Exception ex)
            {
                Log.Error($"Create client error {ex.Message}");

                return new Response
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Unkhown server error"
                };
            }
        }

        public async Task<Response> DeleteClient(int id)
        {
            try
            {
                var client = await clientRepository.GetClientById(id);

                if(client.Id == 0)
                {
                    Log.Debug($"Client wiht id {id} not found");

                    return new Response
                    {
                        StatusCode= StatusCode.NotFound,
                        Description = "Client with this id was not found"
                    };
                }

                if(await clientRepository.DeleteClient(id))
                {
                    Log.Debug($"Deleted account with id {id}");

                    return new Response
                    {
                        StatusCode = StatusCode.Ok,
                        Description = "Delete client success"
                    };
                }
                else
                {
                    Log.Debug($"Error when deleted client with id {id}");

                    return new Response
                    {
                        StatusCode = StatusCode.InternalServerError,
                        Description = "Error when delete client"
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error("Delete client error " + ex.Message);

                return new Response
                {
                    Description = "Unkhown server error",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<Response> GetAllClients()
        {
            try
            {
                var clients = await clientRepository.GetClients();

                return new DataResponse<List<ClientWithTasks>>
                {
                    Data = clients,
                    Description = "Get all client success",
                    StatusCode = StatusCode.Ok
                };
            }
            catch (Exception ex) 
            {
                Log.Error($"Get all clients error {ex.Message}");

                return new Response
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Unkhown server error"
                };
            }
        }

        public async Task<Response> GetClient(int id)
        {
            try
            {
                var client = await clientRepository.GetClientById(id);

                if(client.Id == 0)
                {
                    Log.Debug($"Client with id {id} not found");

                    return new Response
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = $"Client with id {id} have not finded"
                    };
                }

                return new DataResponse<GetClient>
                {
                    StatusCode = StatusCode.Ok,
                    Description = "Get client but id success",
                    Data = new GetClient 
                    {
                        Id = client.Id,
                        Email = client.Email,
                        Name = client.Name,
                        Phone = client.Phone,
                        CreateAt = client.CreatedAt,
                        UpdateAt = client.UpdatedAt
                    }
                };
            }
            catch (Exception ex)
            {
                Log.Error($"Get client by Id {id} error {ex.Message}");

                return new Response
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Unkhown server error"
                };
            }
        }

        public async Task<Response> UpdateClient(UpdateClient updateClient)
        {
            try
            {
                var client = await clientRepository.GetClientById(updateClient.Id);

                if(client.Id == 0)
                {
                    Log.Debug($"Error with updaye client with id {updateClient.Id}," +
                        $"           client not fouend");

                    return new Response
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = "Client has not found"
                    };
                }

                client = await clientRepository.UpdateClient(new Client
                {
                    Id= updateClient.Id,
                    Email = updateClient.Email,
                    Name = updateClient.Name,
                    Phone = updateClient.Phone,
                });

                if (client.Id != 0)
                {
                    Log.Debug($"Update client with id {client.Id} success");

                    return new DataResponse<GetClient>
                    {
                        StatusCode = StatusCode.Ok,
                        Description = "Update client success",
                        Data = new GetClient
                        {
                            Id = client.Id,
                            Name = client.Name,
                            Email = client.Email,
                            Phone = client.Phone,
                            CreateAt = client.CreatedAt,
                            UpdateAt = client.UpdatedAt,
                        }
                    };
                }
                else
                {
                    Log.Debug($"Update client with id {client.Id} error");

                    return new Response
                    {
                        StatusCode = StatusCode.InternalServerError,
                        Description = "Operation execute with error"
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error("Update client error " + ex.Message);

                return new Response
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Unkhown server error"
                };
            }
        }
    }
}
