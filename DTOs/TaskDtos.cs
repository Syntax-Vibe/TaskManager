// DTOs/TaskDtos.cs

// جلوگیری از تداخل با System.Threading.Tasks.TaskStatus
using DomainTaskStatus = TaskManager.Api.Domain.TaskStatus;

namespace TaskManager.Api.DTOs;

public record TaskCreateDto(string Title, string? Description, DateTime? DueDate);
public record TaskUpdateDto(string Title, string? Description, DateTime? DueDate, DomainTaskStatus Status);
public record TaskResponse(Guid Id, string Title, string? Description, DateTime? DueDate, DomainTaskStatus Status, DateTime CreatedAt);
