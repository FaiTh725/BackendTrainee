using MyTask = Trainee.Domain.Entities.Task;
using MyTaskStatus = Trainee.Domain.Enums.TaskStatus;

namespace Trainee.Dal.Interfaces
{
    public interface ITaskRepository
    {
        public Task<MyTask> CreateTask(MyTask task);

        public Task<List<MyTask>> GetAllTasks();

        public Task<List<MyTask>> GetAllTasks(MyTaskStatus status);

        public Task<MyTask> GetTaskById(int id);

        public Task<MyTask> UpdateTask(MyTask task);

        public Task<bool> DeleteTask(int id);
    }
}
