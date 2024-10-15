using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Data.SqlClient;
using Trainee.Dal.Interfaces;
using Trainee.Domain.Enums;
using MyTask = Trainee.Domain.Entities.Task;
using MyTaskStatus = Trainee.Domain.Enums.TaskStatus;

namespace Trainee.Dal.Implementations
{
    public class TaskRepository : DatabaseRepository, ITaskRepository
    {
        public TaskRepository([FromServices] IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<MyTask> CreateTask(MyTask task)
        {
            await connection.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand(
                    "insert into Tasks(Title, Description, Status, ClientId)" +
                    " values (@title, @description, @status, @clientId) " +
                    "declare @Id int = SCOPE_IDENTITY(); " +
                    "select * from Tasks where Id = @Id;", connection);
            sqlCommand.Parameters.AddWithValue("@title", task.Title);
            sqlCommand.Parameters.AddWithValue("@description", task.Description);
            sqlCommand.Parameters.AddWithValue("@status", (int)task.Status);
            sqlCommand.Parameters.AddWithValue("@clientId", task.ClientId);

            SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

            var newTask = new MyTask();

            if(await reader.ReadAsync())
            {
                newTask = new MyTask
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Title = reader["Title"].ToString(),
                    Description = reader["Description"].ToString(),
                    ClientId = Convert.ToInt32(reader["ClientId"]),
                    Status = (MyTaskStatus)Convert.ToInt32(reader["Status"]),
                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                    UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                };
            }

            await connection.CloseAsync();

            return newTask;
        }

        public async Task<bool> DeleteTask(int id)
        {
            await connection.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("delete Tasks where Id = @id", connection); 
            sqlCommand.Parameters.AddWithValue("@id", id);

            var res = await sqlCommand.ExecuteNonQueryAsync();

            await connection.CloseAsync();

            return res != 0;
        }

        public async Task<List<MyTask>> GetAllTasks()
        {
            var tasks = new List<MyTask>(); 

            await connection.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("select * from Tasks", connection);

            SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

            while (await reader.ReadAsync()) 
            {
                tasks.Add(new MyTask
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Title = reader["Title"].ToString(),
                    Description = reader["Description"].ToString(),
                    Status = (MyTaskStatus)Convert.ToInt32(reader["Status"]),
                    ClientId = Convert.ToInt32(reader["ClientId"]),
                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"].ToString()),
                    UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"].ToString())
                });
            }

            await connection.CloseAsync();

            return tasks;
        }

        public async Task<List<MyTask>> GetAllTasks(MyTaskStatus status)
        {
            var tasks = new List<MyTask>();

            await connection.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("select * from Tasks where Status = @status", connection);
            sqlCommand.Parameters.AddWithValue("@status", (int)status);

            SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                tasks.Add(new MyTask
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Title = reader["Title"].ToString(),
                    Description = reader["Description"].ToString(),
                    Status = (MyTaskStatus)Convert.ToInt32(reader["Status"]),
                    ClientId = Convert.ToInt32(reader["ClientId"]),
                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"].ToString()),
                    UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"].ToString())
                });
            }

            await connection.CloseAsync();

            return tasks;
        }

        public async Task<MyTask> GetTaskById(int id)
        {
            var task = new MyTask();

            await connection.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand("select * from Tasks where Id = @id", connection);
            sqlCommand.Parameters.AddWithValue("@id", id);

            SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                task =new MyTask
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Title = reader["Title"].ToString(),
                    Description = reader["Description"].ToString(),
                    Status = (MyTaskStatus)Convert.ToInt32(reader["Status"]),
                    ClientId = Convert.ToInt32(reader["ClientId"]),
                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"].ToString()),
                    UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"].ToString())
                };
            }

            await connection.CloseAsync();

            return task;
        }

        public async Task<MyTask> UpdateTask(MyTask task)
        {
            await connection.OpenAsync();

            SqlCommand sqlCommand = new SqlCommand(
                "update Tasks " +
                "set Title = @title, Description = @description, Status = @status " +
                "where Id = @id" +
                "select * from Tasks where Id = @id", connection);
            sqlCommand.Parameters.AddWithValue("@id", task.Id);
            sqlCommand.Parameters.AddWithValue("@title", task.Title);
            sqlCommand.Parameters.AddWithValue("@description", task.Description);
            sqlCommand.Parameters.AddWithValue("@status", (int)task.Status);

            SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

            var updatedTask = new MyTask();

            if(await reader.ReadAsync())
            {
                updatedTask = new MyTask
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Title = reader["Title"].ToString(),
                    Description = reader["Description"].ToString(),
                    ClientId = Convert.ToInt32(reader["ClientId"]),
                    Status = (MyTaskStatus)Convert.ToInt32(reader["Status"]),
                    CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                    UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"])
                };
            }

            await connection.CloseAsync();

            return updatedTask;
        }
    }
}
