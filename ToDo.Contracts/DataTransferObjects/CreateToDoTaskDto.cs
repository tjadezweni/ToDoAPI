using System.ComponentModel.DataAnnotations;

namespace ToDo.Contracts.DataTransferObjects
{
    public record CreateToDoTaskDto
    {
        [Required]
        [StringLength(20)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Description { get; set; } = null!;
    }
}
