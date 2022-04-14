namespace ToDo.Contracts.DataTransferObjects
{
    public record ToDoTaskDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public bool IsCompleted { get; set; }
    }
}
