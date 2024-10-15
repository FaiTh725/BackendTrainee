using Trainee.Dal.Interfaces;
using Trainee.Domain.Enums;
using Trainee.Domain.Responses;
using Trainee.Models.Task;
using Trainee.Services.Interfaces;

using MyTaskStatus = Trainee.Domain.Enums.TaskStatus;
using MyTask = Trainee.Domain.Entities.Task;
using Serilog;
using Microsoft.OpenApi.Writers;

namespace Trainee.Services.Implementations
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository taskRepository;
        private readonly IClientRepository clientRepository;

        public TaskService(
            ITaskRepository taskRepository,
            IClientRepository clientRepository)
        {
            this.taskRepository = taskRepository;
            this.clientRepository = clientRepository;
        }

        public async Task<Response> CreateTask(CreateTask task)
        {
            try
            {
                var cleint = await clientRepository.GetClientById(task.ClientId);

                if(cleint.Id == 0)
                {
                    Log.Debug($"Client with id {task.ClientId} not found");

                    return new Response
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = "Client with thid id was not found"
                    };
                }

                var newTask = await taskRepository.CreateTask(new MyTask
                        {
                            ClientId = task.ClientId,
                            Description = task.Description,
                            Title = task.Title,
                            Status = task.Status,
                        });

                if(newTask.Id != 0)
                {
                    Log.Debug($"Create task with id {newTask.Id}");

                    return new DataResponse<GetTask>
                    {
                        StatusCode = StatusCode.Ok,
                        Description = "Success create task",
                        Data = new GetTask
                        {
                            Id = newTask.Id,
                            Description = newTask.Description,
                            Title = newTask.Title,
                            Status = newTask.Status,
                            ClientId = newTask.ClientId,
                            CreateAt = newTask.CreatedAt,
                            UpdateAt = newTask.UpdatedAt,
                        }
                    };
                }
                else
                {
                    Log.Debug($"Error when create task");

                    return new Response
                    {
                        StatusCode = StatusCode.InternalServerError,
                        Description = "Error with creating task"
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error create task {ex.Message}");

                return new Response
                {
                    Description = "Unknown server error",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<Response> DeleteTask(int id)
        {
            try
            {
                var task = await taskRepository.GetTaskById(id);

                if (task.Id == 0)
                {
                    Log.Debug($"task with id {id} not found");

                    return new Response
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = "Client with thid id was not found"
                    };
                }

                var isSuccess = await taskRepository.DeleteTask(id);

                if (isSuccess)
                {
                    Log.Debug($"Delete task with id {id}");

                    return new Response
                    {
                        StatusCode = StatusCode.Ok,
                        Description = "Delete task success"
                    };
                }
                else
                {
                    Log.Debug($"Error with deleted task with id {id}");

                    return new Response
                    {
                        StatusCode = StatusCode.InternalServerError,
                        Description = "Error when deleting task"
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error when delete task {ex.Message}");

                return new Response
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Unkhown server error"
                };
            }
        }

        public async Task<DataResponse<List<GetTask>>> GetAllTasks()
        {
            try
            {
                var tasks = await taskRepository.GetAllTasks();

                return new DataResponse<List<GetTask>>
                {
                    StatusCode = StatusCode.Ok,
                    Description = "Success get all tasks",
                    Data = tasks.Select(x => new GetTask
                    {
                        Id = x.Id,
                        Description = x.Description,
                        Title = x.Title,
                        Status = x.Status,
                        CreateAt = x.CreatedAt,
                        UpdateAt = x.UpdatedAt,
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                Log.Error($"Error get all tasks {ex.Message}");

                return new DataResponse<List<GetTask>>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Unkhown server error",
                    Data = new List<GetTask>()
                };
            }
        }

        public async Task<DataResponse<List<GetTask>>> GetAllTasks(MyTaskStatus status)
        {
            try
            {
                var tasks = await taskRepository.GetAllTasks(status);

                return new DataResponse<List<GetTask>>
                {
                    StatusCode = StatusCode.Ok,
                    Description = "Success get all tasks",
                    Data = tasks.Select(x => new GetTask
                    {
                        Id = x.Id,
                        Description = x.Description,
                        Title = x.Title,
                        Status = x.Status,
                        CreateAt = x.CreatedAt,
                        UpdateAt = x.UpdatedAt,
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                Log.Error($"Error when get all tasks {ex.Message}");

                return new DataResponse<List<GetTask>>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Unkhown server error",
                    Data = new List<GetTask>()
                };
            }
        }

        public async Task<DataResponse<GetTask>> GetTask(int id)
        {
            try
            {
                var task = await taskRepository.GetTaskById(id);

                if(task.Id != 0)
                {

                    return new DataResponse<GetTask>
                    {
                        StatusCode = StatusCode.Ok,
                        Description = "Success get task",
                        Data = new GetTask
                        {
                            Id = task.Id,
                            Description = task.Description,
                            Title = task.Title,
                            Status = task.Status,
                            CreateAt = task.CreatedAt,
                            UpdateAt = task.UpdatedAt,
                        }
                    };
                }
                else
                {
                    Log.Debug($"Error when get task with id {task.Id}");

                    return new DataResponse<GetTask>
                    {
                        Description = "Task with this id was not found",
                        StatusCode = StatusCode.NotFound,
                        Data = new GetTask()
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Error get task {ex.Message}");

                return new DataResponse<GetTask>
                {
                    Description = "Unkhown server error",
                    StatusCode = StatusCode.InternalServerError,
                    Data = new GetTask()
                };
            }
        }

        public async Task<Response> UpdateTask(UpdateTask task)
        {
            try
            {
                var databaseTask = await taskRepository.GetTaskById(task.Id);

                if(databaseTask.Id == 0)
                {
                    Log.Debug($"Task with Id {task.Id} not found");

                    return new Response
                    {
                        StatusCode = StatusCode.NotFound,
                        Description = "Task not found"
                    };
                }

                var updatedTask = await taskRepository.UpdateTask(new MyTask
                {
                    Id = task.Id,   
                    Description = task.Description,
                    Title = task.Title,
                    Status = task.Status,
                });

                if(updatedTask.Id != 0)
                {
                    Log.Debug($"Update Task with id {task.Id}");

                    return new DataResponse<GetTask>
                    {
                        Data = new GetTask
                        {
                            Id = updatedTask.Id,
                            Description = updatedTask.Description,
                            Title = updatedTask.Title,
                            Status = updatedTask.Status,
                            ClientId = updatedTask.ClientId,
                            CreateAt = updatedTask.CreatedAt,
                            UpdateAt = updatedTask.UpdatedAt
                        },
                        StatusCode = StatusCode.Ok,
                        Description = "Success update task"
                    };
                }
                else
                {
                    Log.Debug($"Update task with id {task.Id} error");

                    return new Response
                    {
                        StatusCode = StatusCode.InternalServerError,
                        Description = "Error when updated task"
                    };
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error when update task with " + ex.Message);

                return new Response
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Unkhown server error"
                };
            }
        }
    }
}
