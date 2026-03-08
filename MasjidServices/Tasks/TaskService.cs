using GravyFoodsApi.Data;
using GravyFoodsApi.Models.Task;
using Microsoft.EntityFrameworkCore;
using System;

namespace GravyFoodsApi.MasjidServices.Tasks
{
    public class TaskInfoService
    {
        private readonly MasjidDBContext _context;

        public TaskInfoService(MasjidDBContext context)
        {
            _context = context;
        }

        public async Task<List<TaskInfo>> GetAll()
        {
            return await _context.TaskInfo
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();
        }

        public async Task<TaskInfo> GetById(int id)
        {
            return await _context.TaskInfo.FindAsync(id);
        }

        public async Task Create(TaskInfo task)
        {
            _context.TaskInfo.Add(task);
            await _context.SaveChangesAsync();
        }

        public async Task Update(TaskInfo task)
        {
            _context.TaskInfo.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var task = await _context.TaskInfo.FindAsync(id);
            if (task != null)
            {
                _context.TaskInfo.Remove(task);
                await _context.SaveChangesAsync();
            }
        }
    }
}
