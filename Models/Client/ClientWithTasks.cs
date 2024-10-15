using Trainee.Models.Task;

namespace Trainee.Models.Client
{
    public class ClientWithTasks : GetClient
    {
        public List<GetTask> Tasks { get; set; } = new List<GetTask>();
    }
}
