namespace TaskFlow.Models;

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreateTime { get; } = DateTime.Now;

    protected TaskItem(string title, string description)
    {
        Title = title;
        Description = description;
    }

    public void Complete()
    {
        IsCompleted = true;
    }

    public virtual string Details()
    {
        var status = IsCompleted? "Done" : "Open";
        return $"#{Id} [{GetType().Name}] {Title}-{status}";
    }
}