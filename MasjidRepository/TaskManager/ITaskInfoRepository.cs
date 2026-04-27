using GravyFoodsApi.Models.DTOs.TaskManager;
using GravyFoodsApi.Models.TaskManager;

namespace GravyFoodsApi.MasjidRepository.TaskManager
{
    public interface ITaskInfoRepository
    {

        Task<List<TaskInfoDto>> GetAll();
        Task<List<TaskInfoDto>> GetTasksByDateRage(string strSearch, DateTime fromDate, DateTime toDate);
        Task<TaskInfoDto> GetById(int id);
        Task<bool> Create(TaskInfoDto _dto);
        Task<bool> CreateTaskLogAsync(TasksLogDto _dto);
        Task<bool> CopyTask(TaskInfoDto _dto);
        Task<bool> Update(TaskInfoDto _dto);
        Task<bool> Delete(int id);
    }
}
