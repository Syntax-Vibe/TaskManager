using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Data;
using TaskManager.Api.Domain;
using TaskManager.Api.DTOs;
// رفع تداخل نام با System.Threading.Tasks.TaskStatus
using DomainTaskStatus = TaskManager.Api.Domain.TaskStatus;

namespace TaskManager.Api.Services;

public interface ITaskService
{
    Task<TaskResponse> CreateAsync(Guid userId, TaskCreateDto dto);
    Task<List<TaskResponse>> GetAsync(Guid userId, int page = 1, int pageSize = 20, string? status = null);
    Task<TaskResponse?> GetByIdAsync(Guid userId, Guid id);
    Task<TaskResponse> UpdateAsync(Guid userId, Guid id, TaskUpdateDto dto);
    Task DeleteAsync(Guid userId, Guid id);
}

public class TaskService : ITaskService
{
    private readonly AppDbContext _db;
    public TaskService(AppDbContext db) => _db = db;

    public async Task<TaskResponse> CreateAsync(Guid userId, TaskCreateDto dto)
    {
        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            UserId = userId
        };
        _db.Tasks.Add(task);
        await _db.SaveChangesAsync();
        return Map(task);
    }

    public async Task<List<TaskResponse>> GetAsync(Guid userId, int page = 1, int pageSize = 20, string? status = null)
    {
        var q = _db.Tasks.AsNoTracking().Where(t => t.UserId == userId);

        // مهم: از alias استفاده کن تا تداخل پیش نیاد
        if (!string.IsNullOrWhiteSpace(status) &&
            Enum.TryParse<DomainTaskStatus>(status, true, out var st))
        {
            q = q.Where(t => t.Status == st);
        }

        var list = await q.OrderByDescending(t => t.CreatedAt)
                          .Skip((page - 1) * pageSize)
                          .Take(pageSize)
                          .ToListAsync();

        return list.Select(Map).ToList();
    }

    public async Task<TaskResponse?> GetByIdAsync(Guid userId, Guid id)
    {
        var t = await _db.Tasks.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id);
        return t is null ? null : Map(t);
    }

    public async Task<TaskResponse> UpdateAsync(Guid userId, Guid id, TaskUpdateDto dto)
    {
        var t = await _db.Tasks.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id)
                ?? throw new KeyNotFoundException("Task not found");
        t.Title = dto.Title;
        t.Description = dto.Description;
        t.DueDate = dto.DueDate;
        t.Status = dto.Status;
        await _db.SaveChangesAsync();
        return Map(t);
    }

    public async Task DeleteAsync(Guid userId, Guid id)
    {
        var t = await _db.Tasks.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id)
                ?? throw new KeyNotFoundException("Task not found");
        _db.Tasks.Remove(t);
        await _db.SaveChangesAsync();
    }

    private static TaskResponse Map(TaskItem t) =>
        new(t.Id, t.Title, t.Description, t.DueDate, t.Status, t.CreatedAt);
}

