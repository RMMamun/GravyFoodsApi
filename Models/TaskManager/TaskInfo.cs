namespace GravyFoodsApi.Models.TaskManager
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class TaskInfo
    {
        [Key]
        public int Id { get; set; }

        public string ProjectId { get; set; }
        public int TaskId { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }
        public bool IsCopied { get; set; }
        public bool IsShifted { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime? DueDate { get; set; }

        public int OrderNo { get; set; }

        public int ProposedTimeInMinutes { get; set; } = 0;

        public string CompanyId { get; set; }
        public string BranchId { get; set; }

        // Navigation
        public ICollection<TasksLog> TasksLogs { get; set; }
    }
}
