using Microsoft.AspNetCore.Mvc;
using ToDo.Contracts.DataTransferObjects;
using ToDo.Contracts.Services;
using ToDoAPI.ActionFilters;

namespace ToDoAPI.Controllers
{
    [Route("api/toDoTask")]
    [ApiController]
    public class ToDoTaskController : ControllerBase
    {
        private readonly IToDoTaskService _toDoTaskService;

        public ToDoTaskController(IToDoTaskService toDoTaskService)
        {
            _toDoTaskService = toDoTaskService;
        }

        [HttpGet(Name = "GetAllToDoTasks")]
        public async Task<IActionResult> GetAllToDoTasks(
            [FromQuery] int pageNumber = 1, [FromQuery] int pageCount = 20)
        {
            var toDoTaskList = await _toDoTaskService.GetAllAsync(pageNumber, pageCount);
            return Ok(toDoTaskList);
        }

        [HttpGet("{toDoTaskId:int}", Name = "GetToDoTaskById")]
        public async Task<IActionResult> GetToDoTaskById(int toDoTaskId)
        {
            var toDotask = await _toDoTaskService.GetToDoTaskByIdAsync(toDoTaskId);
            return Ok(toDotask);
        }

        [HttpPost(Name = "CreateToDoTask")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateToDoTask([FromBody] CreateToDoTaskDto dto)
        {
            var toDoTask = await _toDoTaskService.CreateToDoTaskAsync(dto);
            return Created("GetToDoTaskById", toDoTask);
        }

        [HttpPut("{toDoTaskId:int}", Name = "UpdateToDoTaskById")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateToDoTask(int toDoTaskId, [FromBody] UpdateToDoTaskDto dto)
        {
            var toDoTask = await _toDoTaskService.UpdateToDoTaskDetailsAsync(toDoTaskId, dto);
            return Ok(toDoTask);
        }

        [HttpDelete("{toDoTaskId:int}", Name = "DeleteToDoTaskById")]
        public async Task<IActionResult> Delete(int toDoTaskId)
        {
            await _toDoTaskService.DeleteToDoTaskAsync(toDoTaskId);
            return NoContent();
        }
    }
}
