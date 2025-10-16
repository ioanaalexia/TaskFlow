using System.Globalization;

namespace TaskFlow.Models;

public class Project
{
    public string Name { get; set; }
    private readonly List<TaskItem> _tasks = new();

    private int _idNext = 1;

    public Project(string name)
    {
        Name = name;
    }

    public T AddTask<T>(T task) where T : TaskItem
    {
        task.Id=_idNext++;
        _tasks.Add(task);
        return task;
    }

    public bool CompleteTask(int id)
    {
        foreach (TaskItem t in _tasks)
        {
            if (t.Id == id)
            {
                t.Complete();
                return true;
            }
        }

        return false;
    }

    public IReadOnlyList<TaskItem> ListTask()
    {
        return _tasks;
    }
}