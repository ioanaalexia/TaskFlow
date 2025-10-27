namespace TaskFlow.Models;

public sealed class WorkTask : TaskItem
{
    public string? Assignee { get; set; }
    
    public WorkTask(string title, string description, string? assignee, TaskPriority priority, TaskStatus status, DueDate dueDate) : base(title, description, priority, status, dueDate)
    {
        Assignee = assignee;
    }

    public override string ToString()
    {
        var person = string.IsNullOrWhiteSpace(Assignee) ? "(unassigned)" : Assignee;
        return base.ToString() + $" | Assignee: {person} | Priority: {Priority} | DueDate: {DueDate.Value}";
    }
}
