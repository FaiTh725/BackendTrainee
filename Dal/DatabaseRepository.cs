using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using DotNetEnv;

namespace Trainee.Dal
{
    public class DatabaseRepository
    {
        protected SqlConnection connection;

        public DatabaseRepository([FromServices]IConfiguration configuration)
        {
            //Env.Load();

            //string connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__MSSQLConnection") ?? "";

            //if(string.IsNullOrEmpty(connectionString))
            //{
            //    connection = new SqlConnection(connectionString);
            //}

            string connectionString = configuration.GetConnectionString("MSSQLConnection") ?? string.Empty;

            if (connectionString != string.Empty)
            {
                connection = new SqlConnection(connectionString);
            }
        }
    }
}
