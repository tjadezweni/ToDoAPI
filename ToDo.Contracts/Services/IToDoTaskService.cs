using ToDo.Contracts.DataTransferObjects;

namespace ToDo.Contracts.Services
{
    public interface IToDoTaskService
    {
        Task<ToDoTaskDto> CreateToDoTaskAsync(CreateToDoTaskDto dto);

        Task<ToDoTaskDto> UpdateToDoTaskDetailsAsync(int toDoTaskId, UpdateToDoTaskDto dto);

        Task DeleteToDoTaskAsync(int toDoTaskId);

        Task<ToDoTaskDto> GetToDoTaskByIdAsync(int toDoTaskId);

        PagedList<ToDoTaskDto> GetAllAsync(int pageNumber, int pageCount);
    }
}
