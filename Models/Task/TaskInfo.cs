namespace GravyFoodsApi.Models.Task
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using System.ComponentModel.DataAnnotations;

    public class TaskInfo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime? DueDate { get; set; }

        public string CompanyId { get; set; }
        public string BranchId { get; set; }


    }
}
