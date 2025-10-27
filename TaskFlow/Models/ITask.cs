namespace TaskFlow.Models;

public interface ITask
{
    int Id { get; }
    string Title { get; }
    string Description { get; } 
    TaskPriority  Priority { get; }
    
    TaskStatus Status { get; }
    
    void Complete();
    string Details();
}