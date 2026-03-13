using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.MasjidRepository.TaskManager;
using GravyFoodsApi.Models.DTOs.TaskManager;
using GravyFoodsApi.Models.TaskManager;
using Microsoft.EntityFrameworkCore;
using System;

namespace GravyFoodsApi.MasjidServices.TaskManager
{
    public class TaskInfoService : ITaskInfoRepository
    {
        private readonly MasjidDBContext _context;
        private readonly ITenantContextRepository _tenant;
        public TaskInfoService(MasjidDBContext context, ITenantContextRepository tenant)
        {
            _context = context;
            _tenant = tenant;
        }

        public async Task<List<TaskInfoDto>> GetAll()
        {
            try
            {
                //.Where(x => x.BranchId == _tenant.BranchId && x.CompanyId == _tenant.CompanyId)
                var result = await _context.TaskInfo
                    
                    .OrderByDescending(x => x.CreatedDate)
                    .ToListAsync();

                var allTasks = result.Select(x => new TaskInfoDto
                {
                    Id = x.Id,
                    TaskId = x.TaskId,
                    ProjectId = x.ProjectId,
                    Title = x.Title,
                    Description = x.Description,
                    IsCompleted = x.IsCompleted,
                    CreatedDate = x.CreatedDate,
                    StartDate = x.StartDate,
                    DueDate = x.DueDate,
                    OrderNo = x.OrderNo
                }).ToList();

                return allTasks;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
                Console.WriteLine($"Error retrieving tasks: {ex.Message}");
                return new List<TaskInfoDto>();
            }

        }

        public async Task<TaskInfoDto> GetById(int id)
        {
            var result = await _context.TaskInfo.FindAsync(id);

            if (result == null)
            {
                return null;
            }

            var taskInfoDto = new TaskInfoDto
            {
                Id = result.Id,
                TaskId = result.TaskId,
                ProjectId = result.ProjectId,
                Title = result.Title,
                Description = result.Description,
                IsCompleted = result.IsCompleted,
                CreatedDate = result.CreatedDate,
                StartDate = result.StartDate,
                DueDate = result.DueDate,
                OrderNo = result.OrderNo
            };

            return taskInfoDto;
        }

        public async Task<bool> Create(TaskInfoDto taskInfo)
        {
            try
            {
                var existingTask = await _context.TaskInfo
                    .FirstOrDefaultAsync(t => t.Title == taskInfo.Title && t.Description == taskInfo.Description);
                if (existingTask == null)
                {
                    var newTaskInfo = new TaskInfo
                    {
                        TaskId = await GetTaskId(),
                        ProjectId = taskInfo.ProjectId,
                        Title = taskInfo.Title,
                        Description = taskInfo.Description,
                        IsCompleted = taskInfo.IsCompleted,
                        CreatedDate = taskInfo.CreatedDate,
                        StartDate = taskInfo.StartDate,
                        DueDate = taskInfo.DueDate,
                        OrderNo = taskInfo.OrderNo,
                        BranchId = _tenant.BranchId,
                        CompanyId = _tenant.CompanyId
                    };

                    _context.TaskInfo.Add(newTaskInfo);
                    await _context.SaveChangesAsync();

                    return true;
                }
                else
                {
                    Console.WriteLine($"Task with title '{taskInfo.Title}' already exists.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
                Console.WriteLine($"Error creating task: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CopyTask(TaskInfoDto taskInfo)
        {
            try
            {
                var existingTask = await _context.TaskInfo
                    .FirstOrDefaultAsync(t => t.Title == taskInfo.Title && t.Description == taskInfo.Description);
                if (existingTask != null)
                {
                    var newTaskInfo = new TaskInfo
                    {
                        TaskId = taskInfo.TaskId,
                        ProjectId = taskInfo.ProjectId,
                        Title = taskInfo.Title,
                        Description = taskInfo.Description,
                        IsCompleted = taskInfo.IsCompleted,
                        CreatedDate = taskInfo.CreatedDate,
                        StartDate = taskInfo.StartDate,
                        DueDate = taskInfo.DueDate,
                        OrderNo = taskInfo.OrderNo,
                        BranchId = _tenant.BranchId,
                        CompanyId = _tenant.CompanyId
                    };

                    _context.TaskInfo.Add(newTaskInfo);
                    await _context.SaveChangesAsync();

                    return true;
                }
                else
                {
                    Console.WriteLine($"Task with title '{taskInfo.Title}' already exists.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
                Console.WriteLine($"Error creating task: {ex.Message}");
                return false;
            }
        }


        private async Task<int> GetTaskId()
        {
            var lastTask = await _context.TaskInfo
                .OrderByDescending(t => t.TaskId)
                .FirstOrDefaultAsync();
            return lastTask != null ? lastTask.TaskId + 1 : 1;
        }



        public async Task<bool> Update(TaskInfoDto taskInfo)
        {
            try
            {
                var existingTask = await _context.TaskInfo.FindAsync(taskInfo.Id);
                if (existingTask != null)
                {
                    existingTask.TaskId = taskInfo.TaskId;
                    existingTask.ProjectId = taskInfo.ProjectId;

                    existingTask.Title = taskInfo.Title;
                    existingTask.Description = taskInfo.Description;
                    existingTask.IsCompleted = taskInfo.IsCompleted;
                    existingTask.CreatedDate = taskInfo.CreatedDate;
                    existingTask.StartDate = taskInfo.StartDate;
                    existingTask.DueDate = taskInfo.DueDate;
                    existingTask.OrderNo = taskInfo.OrderNo;
                    existingTask.BranchId = _tenant.BranchId;
                    existingTask.CompanyId = _tenant.CompanyId;


                    _context.TaskInfo.Update(existingTask);
                    await _context.SaveChangesAsync();

                    return true;
                }
                else
                {
                    Console.WriteLine($"Task with ID {taskInfo.Id} not found.");
                    return false;
                }

            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
                Console.WriteLine($"Error updating task: {ex.Message}");
                return false;

            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var _task = await _context.TaskInfo.FindAsync(id);
                if (_task != null)
                {
                    _context.TaskInfo.Remove(_task);
                    await _context.SaveChangesAsync();

                    return true;
                }
                else
                {
                    Console.WriteLine($"Task with ID {id} not found.");
                    return false;
                }

            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
                Console.WriteLine($"Error deleting task: {ex.Message}");
                return false;
            }
        

        }
    }
}
