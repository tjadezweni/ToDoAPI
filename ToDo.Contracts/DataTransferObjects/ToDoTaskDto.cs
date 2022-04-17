using System.ComponentModel.DataAnnotations;

namespace ToDo.Contracts.DataTransferObjects
{
    public record ToDoTaskDto
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        public string Description { get; set; } = null!;

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        [Required]
        public bool IsCompleted { get; set; }
    }
}
