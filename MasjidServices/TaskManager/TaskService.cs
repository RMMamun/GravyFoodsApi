using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.MasjidRepository.TaskManager;
using GravyFoodsApi.Models.DTOs.TaskManager;
using GravyFoodsApi.Models.TaskManager;
using Humanizer;
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

        //public async Task<List<TaskInfoDto>> GetAll()
        //{
        //    try
        //    {
        //        // 🔥 1. Load tasks
        //        var tasks = await _context.TaskInfo
        //            .OrderByDescending(x => x.CreatedDate)
        //            .ToListAsync();

        //        // 🔥 2. Load ALL logs once
        //        var logs = await _context.TasksLog.ToListAsync();

        //        // 🔥 3. Group logs by Task Id
        //        var logsByTask = logs
        //            .GroupBy(x => x.Id)
        //            .ToDictionary(g => g.Key, g => g.ToList());

        //        // 🔥 4. Map DTO
        //        var allTasks = tasks.Select(task =>
        //        {
        //            var taskLogs = logsByTask.ContainsKey(task.Id)
        //                ? logsByTask[task.Id]
        //                : new List<TasksLog>();

        //            var elapsedMinutes = taskLogs
        //                .Where(t => t.StartDateTime.HasValue && t.EndDateTime.HasValue)
        //                .Sum(t => EF.Functions.DateDiffMinute(
        //                    t.StartDateTime!.Value,
        //                    t.EndDateTime!.Value));

        //            return new TaskInfoDto
        //            {
        //                Id = task.Id,
        //                TaskId = task.TaskId,
        //                ProjectId = task.ProjectId,
        //                Title = task.Title,
        //                Description = task.Description,
        //                IsCompleted = task.IsCompleted,
        //                CreatedDate = task.CreatedDate,
        //                StartDate = task.StartDate,
        //                DueDate = task.DueDate,
        //                OrderNo = task.OrderNo,
        //                IsCopied = task.IsCopied,
        //                IsShifted = task.IsShifted,

        //                ProposedTimeInMinutes = task.ProposedTimeInMinutes,
        //                ElapsedInMinutes = elapsedMinutes,

        //                TasksLogs = taskLogs.Select(x => new TasksLogDto
        //                {
        //                    SLNo = x.SLNo,
        //                    Id = x.Id,
        //                    StartDateTime = x.StartDateTime,
        //                    EndDateTime = x.EndDateTime
        //                }).ToList()
        //            };
        //        }).ToList();

        //        return allTasks;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error retrieving tasks: {ex.Message}");
        //        return new List<TaskInfoDto>();
        //    }
        //}


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
                    OrderNo = x.OrderNo,
                    IsCopied = x.IsCopied,
                    IsShifted = x.IsShifted,
                    TasksLogs = _context.TasksLog?.Select(x => new TasksLogDto
                    {
                        SLNo = x.SLNo,
                        Id = x.Id,
                        StartDateTime = x.StartDateTime,
                        EndDateTime = x.EndDateTime
                    }).ToList(),
                    ProposedTimeInMinutes = Math.Round(((int)x.ProposedTimeInMinutes / 60m),2),

                    ElapsedInMinutes = Math.Round((_context.TasksLog
                    .Where(t => t.Id == x.Id && t.StartDateTime.HasValue && t.EndDateTime.HasValue)
                    .Sum(t => EF.Functions.DateDiffMinute(t.StartDateTime.Value, t.EndDateTime.Value)) / 60m),2)



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

            // 🔥 Load logs ONCE (filtered)
            var logs = await _context.TasksLog
                .Where(t => t.Id == id)
                .ToListAsync();

            // 🔥 Calculate elapsed time
            var elapsedMinutes = logs
                .Where(t => t.StartDateTime.HasValue && t.EndDateTime.HasValue)
                .Sum(t => EF.Functions.DateDiffMinute(
                    t.StartDateTime!.Value,
                    t.EndDateTime!.Value
                ));

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
                OrderNo = result.OrderNo,
                IsCopied = result.IsCopied,
                IsShifted = result.IsShifted,
                TasksLogs = logs.Select(x => new TasksLogDto
                {
                    SLNo = x.SLNo,
                    Id = x.Id,
                    StartDateTime = x.StartDateTime,
                    EndDateTime = x.EndDateTime
                }).ToList(),

                ProposedTimeInMinutes = result.ProposedTimeInMinutes,

                ElapsedInMinutes = _context.TasksLog
                    .Where(t => t.Id == result.Id && t.StartDateTime.HasValue && t.EndDateTime.HasValue)
                    .Sum(t => EF.Functions.DateDiffMinute(t.StartDateTime.Value, t.EndDateTime.Value))


            };

            // 🔥 Convert minutes → hours (if needed)
            if (taskInfoDto.ProposedTimeInMinutes is > 0)
            {
                taskInfoDto.ProposedTimeInMinutes =
                    Math.Round(taskInfoDto.ProposedTimeInMinutes ?? 0 / 60m, 2);
            }

            if (taskInfoDto.ElapsedInMinutes is > 0)
            {
                taskInfoDto.ElapsedInMinutes =
                    Math.Round(taskInfoDto.ElapsedInMinutes ?? 0 / 60m, 2);
            }

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
                        CompanyId = _tenant.CompanyId,
                        IsShifted = taskInfo.IsShifted,
                        IsCopied = taskInfo.IsCopied,
                        ProposedTimeInMinutes =  (int)taskInfo.ProposedTimeInMinutes
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

        public async Task<bool> CreateTaskLogAsync(TasksLogDto _dto)
        {
            try
            {
                var newTaskLog = new TasksLog
                {
                    Id = _dto.Id,
                    StartDateTime = _dto.StartDateTime,
                    EndDateTime = _dto.EndDateTime,
                    

                };
                _context.TasksLog.Add(newTaskLog);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
                Console.WriteLine($"Error creating task log: {ex.Message}");
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
                        CompanyId = _tenant.CompanyId,
                        IsCopied = taskInfo.IsCopied,
                        IsShifted = taskInfo.IsShifted,
                        ProposedTimeInMinutes = (int)taskInfo.ProposedTimeInMinutes  
                        
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
                    existingTask.IsCopied = taskInfo.IsCopied;
                    existingTask.IsShifted = taskInfo.IsShifted;
                    existingTask.ProposedTimeInMinutes = (int)taskInfo.ProposedTimeInMinutes;



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
