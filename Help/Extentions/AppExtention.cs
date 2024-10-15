using DotNetEnv;
using Serilog;
using System.Data.SqlClient;

namespace Trainee.Help.Extentions
{
    public static class AppExtention
    {
        public static void MigrateDatabase(this WebApplicationBuilder builder)
        {
            string masterConnection = builder.Configuration.GetConnectionString("CreateDatabaseConnection");
            string connectionString = builder.Configuration.GetConnectionString("MSSQLConnection");

            try
            {
                string rootPath = AppContext.BaseDirectory;

                string createDatabaseScript = File.ReadAllText(Path.Combine(rootPath, "Scripts/CreateDatabase.sql"));
                string createTablesScript = File.ReadAllText(Path.Combine(rootPath, "Scripts/CreateTables.sql"));
                //string createTriggers = File.ReadAllText(Path.Combine(rootPath, "Scripts/CreateTriggers.sql"));

                bool isExist = false;

                // Create Database
                using (SqlConnection connection = new SqlConnection(masterConnection))
                {
                    connection.Open();

                    using SqlCommand commandGetDataBase = new SqlCommand("select name from sys.databases where name = 'Trainee'", connection);

                    using (SqlDataReader reader = commandGetDataBase.ExecuteReader())
                    {
                        isExist = reader.HasRows; 
                    }    


                    // Database not exist than create new
                    if (!isExist)
                    {
                        using SqlCommand sqlCommand = new SqlCommand(createDatabaseScript, connection);
                
                        sqlCommand.ExecuteNonQuery();

                        Log.Information("Create database");
                    }
                        
                }

                if(!isExist)
                {
                    // TODO: may be check to exist tables
                    // Create tables for creating Database
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string clientInsertedTrigger = File.ReadAllText("Scripts/ClientInserted.sql");
                        string clientUpdatedTrigger = File.ReadAllText("Scripts/ClientsUpdate.sql");
                        string taskInsertedTrigger = File.ReadAllText("Scripts/TaskInserted.sql");
                        string taskUpdatedTrigger = File.ReadAllText("Scripts/TaskUpdated.sql");

                        connection.Open();

                        using SqlCommand sqlCommand = new SqlCommand(createTablesScript, connection);

                        sqlCommand.ExecuteNonQuery();

                        Log.Information("Create tables");

                        sqlCommand.CommandText = clientInsertedTrigger;
                        sqlCommand.ExecuteNonQuery();
                        Log.Information("Create client inserted trigger");

                        sqlCommand.CommandText = clientUpdatedTrigger;
                        sqlCommand.ExecuteNonQuery();
                        Log.Information("Create client updated trigger");

                        sqlCommand.CommandText = taskInsertedTrigger;
                        sqlCommand.ExecuteNonQuery();
                        Log.Information("Create task inserted trigger");

                        sqlCommand.CommandText = taskUpdatedTrigger;
                        sqlCommand.ExecuteNonQuery();
                        Log.Information("Create task updated trigger");

                        //using SqlCommand sqlCommand1 = new SqlCommand(createTriggers, connection);

                        //sqlCommand1.ExecuteNonQuery();

                        //Log.Information("Create triggers");
                    }
                }

            }
            catch (Exception ex) 
            {

                Log.Error($"Error migrating database\n{ex.Message}");
            }
        }

        public static void ConfigureAppSetting(this WebApplicationBuilder builder)
        {
            string envFileName = builder.Environment.IsDevelopment() ? "Secrets.env" : "SecretsPublish.env";

            if(File.Exists(envFileName))
            {
                var insideFile = File.ReadAllText(envFileName);
                Log.Information($"Env file conf \n" +
                                $"{insideFile}");
            }
            else
            {
                Log.Error("Env file not found");
            }

            Env.Load(envFileName);

            var connectionStringCreateDb = Environment.GetEnvironmentVariable("ConnectionStrings__CreateDatabaseConnection");
            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__MSSQLConnection");

            if (string.IsNullOrEmpty(connectionString) 
                || string.IsNullOrEmpty(connectionStringCreateDb))
            {
                throw new Exception("Invironment variables was not load");
            }

            builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>
            {
                {"ConnectionStrings:MSSQLConnection", connectionString },
                {"ConnectionStrings:CreateDatabaseConnection", connectionStringCreateDb }
            });
        }
    }
}
