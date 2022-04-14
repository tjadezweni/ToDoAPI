using AutoMapper;
using ToDo.Contracts.DataTransferObjects;
using ToDo.Infrastructure.Entities;

namespace ToDo.Mappers
{
    internal sealed class ToDoTaskMapperProfile : Profile
    {
        public ToDoTaskMapperProfile()
        {
            CreateMap<CreateToDoTaskDto, ToDoTask>();
            CreateMap<UpdateToDoTaskDto, ToDoTask>();
            CreateMap<ToDoTask, ToDoTaskDto>();
            CreateMap<IEnumerable<ToDoTask>, IEnumerable<ToDoTask>>();
        }
    }
}
