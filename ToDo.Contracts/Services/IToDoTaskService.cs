using ToDo.Contracts.DataTransferObjects;
using ToDo.Contracts.Interfaces;

namespace ToDo.Contracts.Services
{
    public interface IToDoTaskService
    {
        Task<ToDoTaskDto> CreateToDoTaskAsync(CreateToDoTaskDto dto);

        Task<ToDoTaskDto> UpdateToDoTaskDetailsAsync(int toDoTaskId, UpdateToDoTaskDto dto);

        Task DeleteToDoTaskAsync(int toDoTaskId);

        Task<ToDoTaskDto> GetToDoTaskByIdAsync(int toDoTaskId);

        IPagedList<ToDoTaskDto> GetAllAsync(int pageNumber, int pageCount);
    }
}
