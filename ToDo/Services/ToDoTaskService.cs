using AutoMapper;
using ToDo.Common;
using ToDo.Contracts;
using ToDo.Contracts.DataTransferObjects;
using ToDo.Contracts.Exceptions;
using ToDo.Contracts.Interfaces;
using ToDo.Contracts.Services;
using ToDo.Infrastructure;
using ToDo.Infrastructure.Entities;

namespace ToDo.Services
{
    public class ToDoTaskService : IToDoTaskService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public ToDoTaskService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ToDoTaskDto> CreateToDoTaskAsync(CreateToDoTaskDto dto)
        {
            var toDoTask = _mapper.Map<ToDoTask>(dto);
            await _db.ToDoTasks.AddAsync(toDoTask);
            await _db.SaveChangesAsync();
            var toDoTaskDto = _mapper.Map<ToDoTaskDto>(toDoTask);
            return toDoTaskDto;
        }

        public async Task DeleteToDoTaskAsync(int toDoTaskId)
        {
            var toDoTask = await _db.ToDoTasks.FindAsync(toDoTaskId);
            if (toDoTask is null)
            {
                return;
            }
            _db.ToDoTasks.Remove(toDoTask);
            await _db.SaveChangesAsync();
        }

        public IPagedList<ToDoTaskDto> GetAllAsync(int pageNumber, int pageCount)
        {
            var toDoTaskList = _db.ToDoTasks.ToList();
            var toDoTaskDtoList = _mapper.Map<IEnumerable<ToDoTaskDto>>(toDoTaskList);
            return new PagedList<ToDoTaskDto>(toDoTaskDtoList, pageNumber, pageCount);
        }

        public async Task<ToDoTaskDto> GetToDoTaskByIdAsync(int toDoTaskId)
        {
            var toDoTask = await FindToDoTaskAsync(toDoTaskId);
            var toDoTaskDto = _mapper.Map<ToDoTaskDto>(toDoTask);
            return toDoTaskDto;
        }

        public async Task<ToDoTaskDto> UpdateToDoTaskDetailsAsync(int toDoTaskId, UpdateToDoTaskDto dto)
        {
            var toDoTask = await FindToDoTaskAsync(toDoTaskId);
            _mapper.Map(dto, toDoTask);
            await _db.SaveChangesAsync();
            var toDoTaskDto = _mapper.Map<ToDoTaskDto>(toDoTask);
            return toDoTaskDto;
        }

        private async Task<ToDoTask> FindToDoTaskAsync(int toDoTaskId)
        {
            var toDoTask = await _db.ToDoTasks.FindAsync(toDoTaskId);
            if (toDoTask is null)
            {
                throw new ToDoTaskNotFoundException(toDoTaskId);
            }
            return toDoTask;
        }
    }
}
