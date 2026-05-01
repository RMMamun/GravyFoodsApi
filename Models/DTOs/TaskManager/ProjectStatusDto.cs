using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GravyFoodsApi.Models.DTOs.TaskManager
{
    public class ProjectStatusDto
    {
        public string ProjectId { get; set; }
        public int TotalTasks { get; set; } = 0;
        public int CompletedTasks { get; set; } = 0;
        public int PendingTasks { get; set; } = 0;
        public double ProjectedHours { get; set; } = 0;
        public double ElapsedHours { get; set; } = 0;
        public int ProgressByDoneTasks { get; set; } = 0;
        public int ProgressByDoneHours { get; set; } = 0;
    }
}
