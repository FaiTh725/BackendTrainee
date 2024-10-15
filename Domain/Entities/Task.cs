
namespace Trainee.Domain.Entities
{
    public class Task
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Trainee.Domain.Enums.TaskStatus Status { get; set; }

        public int ClientId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
