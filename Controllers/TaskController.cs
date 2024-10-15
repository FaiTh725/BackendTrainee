using Microsoft.AspNetCore.Mvc;
using Trainee.Domain.Enums;
using Trainee.Models.Task;
using Trainee.Services.Interfaces;

namespace Trainee.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService taskService;

        public TaskController(ITaskService taskService)
        {
            this.taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(CreateTask task)
        {
            var result = await taskService.CreateTask(task);

            return new JsonResult(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var result = await taskService.GetAllTasks();

            return new JsonResult(result);
        }

        [HttpGet("status/{status:int}")]
        public async Task<IActionResult> GetTasks(Domain.Enums.TaskStatus status)
        {
            var result = await taskService.GetAllTasks(status);

            return new JsonResult(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var result = await taskService.GetTask(id);

            return new JsonResult(result);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateTask(UpdateTask task)
        {
            var result = await taskService.UpdateTask(task);

            return new JsonResult(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var result = await taskService.DeleteTask(id);

            return new JsonResult(result);
        }
    }
}
