namespace GravyFoodsApi.Models.DTOs.TaskManager
{
    public class TasksLogDto
    {
        public int SLNo { get; set; }

        public int Id { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }
    }
}
