using FluentValidation;
using Trainee.Models.Task;

namespace Trainee.Validators
{
    public class TaskValidator : AbstractValidator<CreateTask>
    {
        public TaskValidator()
        {
            RuleFor(t => t.Title)
                .MaximumLength(20).WithMessage("max title lenght 20 symbols");

            RuleFor(t => t.Description)
                .MaximumLength(50).WithMessage("max title lenght 20 symbols");
        
        }
    }
}
