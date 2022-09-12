using Microsoft.AspNetCore.Mvc;
using Tasks.Domain.Models;
using Tasks.WebAPI.Interfaces;

namespace Tasks.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITaskService taskService, ILogger<TasksController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddTask([FromBody] InsertTaskModel task)
        {
            var result = await _taskService.AddTask(Mappings.Mapper.MapInsertTaskModelToTaskModel(task));
            if (result.Flags.HasSuccess)
            {
                return Ok(result);
            }

            _logger.LogError(result.Message);
            return BadRequest(result);

        }

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var result = await _taskService.GetTasks();
                if (result.Flags.HasSuccess)
                {
                    return Ok(result);
                }

                _logger.LogError(result.Message);
                return BadRequest(result);
        }


        [HttpPut]
        public async Task<IActionResult> UpdateTask(TaskModel updateTask)
        {
            var result = await _taskService.UpdateTask(updateTask);
            if (result.Flags.HasSuccess)
            {
                return Ok(result);
            }

            _logger.LogError(result.Message);
            return BadRequest(result);
        }
    }
}
