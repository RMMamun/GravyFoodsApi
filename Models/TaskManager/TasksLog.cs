using GravyFoodsApi.Models.TaskManager;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GravyFoodsApi.Models.TaskManager
{
    [Table("TasksLog")]
    public class TasksLog
    {
        [Key]
        public int SLNo { get; set; }

        [ForeignKey("TaskInfo")]
        public int Id { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        // Navigation
        public TaskInfo TaskInfo { get; set; }
    }
}



