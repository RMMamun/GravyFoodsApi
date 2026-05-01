using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.MasjidRepository.TaskManager;
using GravyFoodsApi.Models.DTOs.TaskManager;
using GravyFoodsApi.Models.TaskManager;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
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
                //var result = await _context.TaskInfo

                //    .OrderByDescending(x => x.CreatedDate)
                //    .ToListAsync();

                //var allTasks = result.Select(x => new TaskInfoDto
                //{
                //    Id = x.Id,
                //    TaskId = x.TaskId,
                //    ProjectId = x.ProjectId,
                //    Title = x.Title,
                //    Description = x.Description,
                //    IsCompleted = x.IsCompleted,
                //    CreatedDate = x.CreatedDate,
                //    StartDate = x.StartDate,
                //    DueDate = x.DueDate,
                //    OrderNo = x.OrderNo,
                //    IsCopied = x.IsCopied,
                //    IsShifted = x.IsShifted,
                //    TasksLogs = _context.TasksLog?.Select(x => new TasksLogDto
                //    {
                //        SLNo = x.SLNo,
                //        Id = x.Id,
                //        StartDateTime = x.StartDateTime,
                //        EndDateTime = x.EndDateTime
                //    }).ToList(),
                //    ProposedTimeInMinutes = Math.Round(((int)x.ProposedTimeInMinutes / 60m),2),
                //    TimeInputType = x.TimeInputType,

                //    ElapsedInMinutes = Math.Round((_context.TasksLog
                //    .Where(t => t.Id == x.Id && t.StartDateTime.HasValue && t.EndDateTime.HasValue)
                //    .Sum(t => EF.Functions.DateDiffMinute(t.StartDateTime.Value, t.EndDateTime.Value)) / 60m),2)



                //}).ToList();




                var allTasks = await _context.TaskInfo
                .OrderByDescending(x => x.CreatedDate)
                .Select(x => new TaskInfoDto
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

                    ProposedTimeInMinutes = Math.Round(((int)x.ProposedTimeInMinutes / 60m), 2),
                    TimeInputType = x.TimeInputType,

                    // Aggregate directly in SQL
                    ElapsedInMinutes = Math.Round(
                        _context.TasksLog
                            .Where(t => t.Id == x.Id && t.StartDateTime != null && t.EndDateTime != null)
                            .Sum(t => (int?)EF.Functions.DateDiffMinute(t.StartDateTime.Value, t.EndDateTime.Value)) ?? 0
                        / 60m, 2
                    )
                })
                .ToListAsync();

                return allTasks;

            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
                Console.WriteLine($"Error retrieving tasks: {ex.Message}");
                return new List<TaskInfoDto>();
            }

        }

        public async Task<List<TaskInfoDto>> GetTasksByDateRage(string strSearch, DateTime fromDate, DateTime toDate)
        {
            try
            {
                //.Where(x => x.BranchId == _tenant.BranchId && x.CompanyId == _tenant.CompanyId)
                var result = await _context.TaskInfo
                    .Where(x => x.CreatedDate.Date >= fromDate.Date && x.CreatedDate.Date <= toDate.Date)
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
                    ProposedTimeInMinutes = Math.Round(((int)x.ProposedTimeInMinutes / 60m), 2),
                    TimeInputType = x.TimeInputType,

                    ElapsedInMinutes = Math.Round((_context.TasksLog
                    .Where(t => t.Id == x.Id && t.StartDateTime.HasValue && t.EndDateTime.HasValue)
                    .Sum(t => EF.Functions.DateDiffMinute(t.StartDateTime.Value, t.EndDateTime.Value)) / 60m), 2)



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
                TimeInputType = result.TimeInputType,

                ElapsedInMinutes = _context.TasksLog
                    .Where(t => t.Id == result.Id && t.StartDateTime.HasValue && t.EndDateTime.HasValue)
                    .Sum(t => EF.Functions.DateDiffMinute(t.StartDateTime.Value, t.EndDateTime.Value))


            };

            // 🔥 Convert minutes → hours (if needed)
            if (taskInfoDto.ProposedTimeInMinutes is > 0)
            {
                taskInfoDto.ProposedTimeInMinutes =
                    Math.Round(taskInfoDto.ProposedTimeInMinutes / 60m, 2);
            }

            if (taskInfoDto.ElapsedInMinutes is > 0)
            {
                taskInfoDto.ElapsedInMinutes =
                    Math.Round(taskInfoDto.ElapsedInMinutes / 60m, 2);
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
                        IsShifted = taskInfo.IsShifted,
                        IsCopied = taskInfo.IsCopied,
                        ProposedTimeInMinutes = (int)taskInfo.ProposedTimeInMinutes,
                        TimeInputType = taskInfo.TimeInputType,

                        BranchId = "1",
                        CompanyId = "1",

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
                        IsCopied = taskInfo.IsCopied,
                        IsShifted = taskInfo.IsShifted,
                        ProposedTimeInMinutes = (int)taskInfo.ProposedTimeInMinutes,
                        TimeInputType = taskInfo.TimeInputType,

                        BranchId = "1",
                        CompanyId = "1",

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
                    existingTask.IsCopied = taskInfo.IsCopied;
                    existingTask.IsShifted = taskInfo.IsShifted;
                    existingTask.ProposedTimeInMinutes = (int)taskInfo.ProposedTimeInMinutes;
                    existingTask.TimeInputType = taskInfo.TimeInputType;

                    existingTask.BranchId = "1";
                    existingTask.CompanyId = "1";

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

        public async Task<ProjectStatusDto?> GetProjectStatusAsync(string projectId)
        {
            try
            {

                var result = await _context.TaskInfo
                .Where(t => t.ProjectId == projectId)

                // STEP 1: Group by TaskId (logical task)
                .GroupBy(t => t.TaskId)

                .Select(g => new
                {
                    TaskId = g.Key,

                    IsCompleted = g.Any(x => x.IsCompleted),

                    ProjectedMinutes = g.Sum(x => x.ProposedTimeInMinutes),

                    // ✅ Calculate elapsed per group WITHOUT using Id list
                    ElapsedMinutes = _context.TasksLog
                        .Where(l =>
                            l.StartDateTime != null &&
                            l.EndDateTime != null &&
                            _context.TaskInfo
                                .Where(t => t.ProjectId == projectId && t.TaskId == g.Key)
                                .Select(t => t.Id)
                                .Contains(l.Id)
                        )
                        .Sum(l => EF.Functions.DateDiffMinute(
                            l.StartDateTime.Value,
                            l.EndDateTime.Value))
                })

                // STEP 2: Aggregate whole project
                .GroupBy(x => 1)

                .Select(g => new ProjectStatusDto
                {
                    ProjectId = projectId,

                    TotalTasks = g.Count(),

                    CompletedTasks = g.Count(x => x.IsCompleted),

                    PendingTasks = g.Count() - g.Count(x => x.IsCompleted),

                    ProjectedHours = g.Sum(x => x.ProjectedMinutes) / 60.0,

                    ElapsedHours = g.Sum(x => x.ElapsedMinutes) / 60.0,

                    ProgressByDoneTasks = g.Count() > 0
                        ? (int)((double)g.Count(x => x.IsCompleted) * 100.0 / g.Count())
                        : 0,

                    ProgressByDoneHours = g.Sum(x => x.ProjectedMinutes) > 0
                        ? (int)(g.Sum(x => x.ElapsedMinutes) * 100.0 / g.Sum(x => x.ProjectedMinutes))
                        : 0
                })
                .FirstOrDefaultAsync();

                return new ProjectStatusDto
                {
                    ProjectId = projectId,
                    TotalTasks = result.TotalTasks,
                    CompletedTasks = result.CompletedTasks,
                    PendingTasks = result.PendingTasks,
                    ElapsedHours = result.ElapsedHours,
                    ProjectedHours = result.ProjectedHours,
                    ProgressByDoneHours = (int)result.ProgressByDoneHours,
                    ProgressByDoneTasks = (int)result.ProgressByDoneTasks,
                };
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
                Console.WriteLine($"Error retrieving project status: {ex.Message}");
                return null;
            }
        }






    }
}
