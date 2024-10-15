using MyTaskStatus = Trainee.Domain.Enums.TaskStatus;

namespace Trainee.Models.Task
{
    public class CreateTask
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty ;

        public MyTaskStatus Status { get; set; } = MyTaskStatus.Pending;

        public int ClientId { get; set; }
    }
}
