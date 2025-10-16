namespace TaskFlow.Models;

public sealed class PersonalTask : TaskItem
{   public string? Person { get; set; }

    public PersonalTask(string title, string description, string? person = null) : base(title, description)
    {
        Person = person;
    }

    public override string Details()
    {
        var person = string.IsNullOrWhiteSpace(Person) ? "self" : Person;
        return base.Details() + $"| For: {person}";
    }
}