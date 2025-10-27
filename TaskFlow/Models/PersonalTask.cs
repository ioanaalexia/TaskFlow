namespace TaskFlow.Models;

public sealed class PersonalTask : TaskItem
{   public string? Person { get; set; }

    public PersonalTask(string title, string description, string? person, TaskPriority priority, TaskStatus status, DueDate dueDate) : base(title, description, priority, status, dueDate)
    {
        Person = person;
    }

    public override string ToString()
    {
        var person = string.IsNullOrWhiteSpace(Person) ? "self" : Person;
        return base.ToString() + $"| For: {person}";
    }
}