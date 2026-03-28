using GravyFoodsApi.Models.TaskManager;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace GravyFoodsApi.Models.DTOs.TaskManager
{
    public class TaskInfoDto
    {

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

        public decimal? ProposedTimeInMinutes { get; set; } = 0;
        public decimal? ElapsedInMinutes { get; set; } = 0;

        // Optional: include logs if needed
        public List<TasksLogDto>? TasksLogs { get; set; }
    }
}
