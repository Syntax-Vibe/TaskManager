
namespace TaskManager.Api.Domain;

public enum TaskStatus { Todo=0, InProgress=1, Done=2 }

public class TaskItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public TaskStatus Status { get; set; } = TaskStatus.Todo;
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
