using FluentValidation;
using FluentValidation.AspNetCore;
using Serilog;
using Trainee.Dal.Implementations;
using Trainee.Dal.Interfaces;
using Trainee.Help.Extentions;
using Trainee.Models.Client;
using Trainee.Models.Task;
using Trainee.Services.Implementations;
using Trainee.Services.Interfaces;
using Trainee.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();

builder.Services.AddControllers()
    .AddFluentValidation(v =>
    {
        v.RegisterValidatorsFromAssemblyContaining<CreateTask>();
        v.RegisterValidatorsFromAssemblyContaining<CreateClient>();
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.ConfigureAppSetting();
builder.MigrateDatabase();

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

//builder.Services.AddScoped<IValidator<CreateClient>, ClientValidator>();
//builder.Services.AddScoped<IValidator<CreateTask>, TaskValidator>();

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<ITaskService, TaskService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
