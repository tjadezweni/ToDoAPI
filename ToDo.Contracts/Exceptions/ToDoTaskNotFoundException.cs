namespace ToDo.Contracts.Exceptions
{
    public sealed class ToDoTaskNotFoundException : NotFoundException
    {
        public ToDoTaskNotFoundException(int toDoTaskId)
            : base(DefaultMessage(toDoTaskId))
        {

        }

        private static string DefaultMessage(int toDoTaskId)
        {
            return $"The To-Do Task with id: {toDoTaskId} does not exist.";
        }
    }
}
