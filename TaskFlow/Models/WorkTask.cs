namespace TaskFlow.Models;
public enum TaskPriority {Low, Medium, High}

public sealed class WorkTask : TaskItem
{
    public string? Assignee { get; set; }
    public TaskPriority Priority { get;}

    public WorkTask(string title, string description, string? assignee, TaskPriority priority) : base(title, description)
    {
        Priority = priority;
        Assignee = assignee;
    }

    public override string Details()
    {
        var person = string.IsNullOrWhiteSpace(Assignee) ? "(unassigned)" : Assignee;
        return base.Details() + $" | Assignee: {person} | Priority: {Priority}";
    }
}
