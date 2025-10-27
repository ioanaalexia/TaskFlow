namespace TaskFlow.Models;

public class TaskNotFoundException: Exception
{
    public TaskNotFoundException(int id): base($"Task {id} was not found.") { }    
}

public class InvalidTaskOperationException : Exception
{
    public InvalidTaskOperationException(string message) : base(message) { }
}