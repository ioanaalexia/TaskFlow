namespace TaskFlow.Models;

public abstract class TaskItem : ITask, IComparable<TaskItem>
{
    public int Id { get; internal set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public TaskPriority Priority { get; private set; }
    public TaskStatus Status { get; private set; }
    
    public DueDate DueDate { get; private set; }

    public override bool Equals(object? obj)
    {
        if (obj is not TaskItem other)
            return false;
        return other.Id == Id;
    }
    
    public override int GetHashCode() => Id.GetHashCode();

    public int CompareTo(TaskItem? other)
    {
        if (other == null)
            return 1;
        
        int dateCompare = DueDate.Value.CompareTo(other.DueDate.Value);
        if (dateCompare != 0)
            return dateCompare;
        
        return Priority.CompareTo(other.Priority);
    }
    
    protected TaskItem(string title, string description, TaskPriority priority, TaskStatus status, DueDate dueDate)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be null or whitespace.", nameof(title));
        if(string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be null or whitespace.", nameof(description));
        
        title = title.Trim();
        description = description.Trim();
        
        if(title.Length == 0)
            throw new ArgumentException("Title cannot be empty.", nameof(title));
        
        if(dueDate is null)
            throw new ArgumentNullException("DueDate cannot be null.", nameof(dueDate));
        
        Title = title;
        Description = description;
        Priority = priority;
        Status = status;
        DueDate = dueDate;
    }

    public void Complete()
    {
        if (Status == TaskStatus.Completed)
        {
            throw new InvalidTaskOperationException("The task is already completed.");
        }
        
        Status = TaskStatus.Completed;
    }

    public virtual string Details() => ToString();
    public override string ToString()
    {
        var status = IsCompleted ? "Done" : "Open";
        return
            $"#{Id} [{GetType().Name}] {Title}-{status}  | Priority: {Priority} | Status: {status} | DueDate: {DueDate.Value}";
    }
}