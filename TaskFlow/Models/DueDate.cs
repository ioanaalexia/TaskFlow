namespace TaskFlow.Models;

public class DueDate
{
    public DateTime Value { get;}

    public DueDate(DateTime value, bool inPast = false)
    {
        if (!inPast && value < DateTime.Today)
        {
            throw new ArgumentException("Due date cannot be in the past");
        }

        Value = value;
    }
    
    public override string ToString() => Value.ToString("yyyy-MM-dd");
}