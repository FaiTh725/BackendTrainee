using Trainee.Domain.Responses;
using Trainee.Models.Task;

using MyTaskStatus = Trainee.Domain.Enums.TaskStatus;

namespace Trainee.Services.Interfaces
{
    public interface ITaskService
    {
        public Task<Response> CreateTask(CreateTask task);

        public Task<DataResponse<List<GetTask>>> GetAllTasks();

        public Task<DataResponse<List<GetTask>>> GetAllTasks(MyTaskStatus status);

        public Task<DataResponse<GetTask>> GetTask(int id);

        public Task<Response> UpdateTask(UpdateTask task);

        public Task<Response> DeleteTask(int id);
    }
}
