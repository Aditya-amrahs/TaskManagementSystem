using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models
{
    public class Task
    {
        [Key]
        public int TaskId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        public string Priority { get; set; } // High, Medium, Low

        public bool IsCompleted { get; set; }

        public DateTime DueDate { get; set; }
    }
}
