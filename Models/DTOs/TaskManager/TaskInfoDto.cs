using System.ComponentModel.DataAnnotations;

namespace GravyFoodsApi.Models.DTOs.TaskManager
{
    public class TaskInfoDto
    {

        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime? DueDate { get; set; }

        public int OrderNo { get; set; }
    }
}
