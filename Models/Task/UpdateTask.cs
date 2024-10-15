using MyTaskStatus = Trainee.Domain.Enums.TaskStatus;

namespace Trainee.Models.Task
{
    public class UpdateTask
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public MyTaskStatus Status { get; set; } = MyTaskStatus.Pending;
    }
}
