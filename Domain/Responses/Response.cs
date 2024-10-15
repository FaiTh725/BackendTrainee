using Trainee.Domain.Enums;

namespace Trainee.Domain.Responses
{
    public class Response
    {
        public string Description { get; set; } = string.Empty;

        public StatusCode StatusCode { get; set; }
    }
}
