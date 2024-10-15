using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Numerics;
using System.Xml.Linq;
using Trainee.Dal.Interfaces;
using Trainee.Domain.Entities;
using Trainee.Models.Client;
using Trainee.Models.Task;

using MyTaskStatus = Trainee.Domain.Enums.TaskStatus;

namespace Trainee.Dal.Implementations
{
    public class ClientRepository : DatabaseRepository, IClientRepository
    {
        public ClientRepository([FromServices]IConfiguration configuration): base(configuration)
        {
        }

        public async Task<Client> CreateClient(Client client)
        {
            await connection.OpenAsync();

            var newClient = new Client();

            var transaction = (SqlTransaction)await connection.BeginTransactionAsync();

            try
            {
                SqlCommand sqlCommand = new SqlCommand(
                @"insert into Clients(Name, Email, Phone) values (@name, @email, @phone);"+
                "declare @Id int = SCOPE_IDENTITY();" +
                "select * from Clients where Id = @Id;",
                connection, transaction);

                sqlCommand.Parameters.AddWithValue("@name", client.Name);
                sqlCommand.Parameters.AddWithValue("@email", client.Email);
                sqlCommand.Parameters.AddWithValue("@phone", client.Phone);

                SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    newClient = new Client()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                        UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"]),
                    };
                }

                await reader.CloseAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
            }
            
            await connection.CloseAsync();

            return newClient;
        }

        public async Task<bool> DeleteClient(int id)
        {
            await connection.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("delete Clients where Id = @id", connection);

            sqlCommand.Parameters.AddWithValue("@id", id);

            var res = await sqlCommand.ExecuteNonQueryAsync();

            await connection.CloseAsync();

            return res != 0;
        }

        public async Task<Client> GetClientById(int id)
        {
            Client client = new Client();

            await connection.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand($"select * from Clients where Id = @id", connection);

            sqlCommand.Parameters.AddWithValue("@id", id);

            SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

            if(await reader.ReadAsync())
            {
                client = new Client
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Email = reader["Email"].ToString(),
                    Phone = reader["Phone"].ToString(),
                    Name = reader["Name"].ToString(),
                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"].ToString()),
                    UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"].ToString())
                };
            }

            await connection.CloseAsync();

            return client;
        }

        public async Task<List<ClientWithTasks>> GetClients()
        {
            var clients = new List<ClientWithTasks>();

            await connection.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("select c.Id, c.Name, c.Email, c.Phone, c.CreatedAt as ClientCreate, c.UpdatedAt as ClientUpdate, " +
                "t.Id as TaskId, t.Title, t.Description, t.Status, t.CreatedAt as TaskCreate, t.UpdatedAt as  TaskUpdate " +
                "from Clients c " +
                "left join Tasks t on t.ClientId = c.Id " +
                "order by c.Id", connection);

            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while(await sqlDataReader.ReadAsync())
            {
                if (clients.Count != 0 
                    && Convert.ToInt32(sqlDataReader["Id"]) == clients[^1].Id)
                {
                    clients[^1].Tasks.Add(new GetTask
                    {
                        Id = Convert.ToInt32(sqlDataReader["TaskId"]),
                        Description = sqlDataReader["Description"].ToString(),
                        Title = sqlDataReader["Title"].ToString(),
                        Status = (MyTaskStatus)Convert.ToInt32(sqlDataReader["Status"]),
                        ClientId =  Convert.ToInt32(sqlDataReader["Id"]),
                        CreateAt = Convert.ToDateTime(sqlDataReader["TaskCreate"].ToString()),
                        UpdateAt = Convert.ToDateTime(sqlDataReader["TaskUpdate"].ToString())
                    });
                }
                else 
                {
                    var client = new ClientWithTasks()
                    {
                        Id = Convert.ToInt32(sqlDataReader["Id"]),
                        Email = sqlDataReader["Email"].ToString(),
                        Phone = sqlDataReader["Phone"].ToString(),
                        Name = sqlDataReader["Name"].ToString(),
                        CreateAt = Convert.ToDateTime(sqlDataReader["ClientCreate"].ToString()),
                        UpdateAt = Convert.ToDateTime(sqlDataReader["ClientUpdate"].ToString())
                    };

                    if (sqlDataReader["TaskId"] != DBNull.Value)
                    {
                        client.Tasks.Add(new GetTask
                        {
                            Id = Convert.ToInt32(sqlDataReader["TaskId"]),
                            Description = sqlDataReader["Description"].ToString(),
                            Title = sqlDataReader["Title"].ToString(),
                            Status = (MyTaskStatus)Convert.ToInt32(sqlDataReader["Status"]),
                            ClientId = client.Id,
                            CreateAt = Convert.ToDateTime(sqlDataReader["TaskCreate"].ToString()),
                            UpdateAt = Convert.ToDateTime(sqlDataReader["TaskUpdate"].ToString())
                        });
                    }

                    clients.Add(client);
                }
            }

            await connection.CloseAsync();

            return clients;
        }

        public async Task<Client> UpdateClient(Client client)
        {
            await connection.OpenAsync();

            var updateClient = new Client();

            using var transaction = (SqlTransaction)await connection.BeginTransactionAsync();

            try
            {
                SqlCommand sqlCommand = new SqlCommand(
                @$"update Clients 
                set Email = @email, Phone = @phone, Name = @name 
                where Id = @id;
                select * from Clients where Id = @id",
                connection, transaction);


                sqlCommand.Parameters.AddWithValue("@id", client.Id);
                sqlCommand.Parameters.AddWithValue("@email", client.Email);
                sqlCommand.Parameters.AddWithValue("@phone", client.Phone);
                sqlCommand.Parameters.AddWithValue("@name", client.Name);

                SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    updateClient = new Client()
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                        UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"]),
                    };
                }

                await reader.CloseAsync();

                await transaction.CommitAsync();
            } 
            catch
            {
                await transaction.RollbackAsync();
            }  

            await connection.CloseAsync();

            return updateClient;
        }
    }
}
