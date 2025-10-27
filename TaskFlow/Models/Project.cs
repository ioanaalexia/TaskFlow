using System.Globalization;
using System.Collections.Generic;
using System.Linq;

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
        if(_tasks.Contains(task))
            throw new InvalidTaskOperationException("Task {task.Id} is already added.");
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

        throw new TaskNotFoundException(id);
    }
    
    public IReadOnlyList<TaskItem> ListTask()
    {
        return _tasks;
    }

    public IReadOnlyList<TaskItem> ListTaskSorted()
    {
        List<TaskItem> copy_list = _tasks.ToList();
        copy_list.Sort();
        return copy_list;
    }

    public IReadOnlyList<TaskItem> ListCompletedTask()
    {
        List<TaskItem> completed = new List<TaskItem>();

        foreach (TaskItem t in _tasks)
        {
            if (t.Status == TaskStatus.Completed)
            {
                completed.Add(t);
            }
        }

        completed.Sort();
        return completed;
    }  

    public IReadOnlyList<TaskItem> ListOverdue()
    {
        DateTime today = DateTime.Today;
        List<TaskItem> overdue = _tasks
            .Where(t=> t.DueDate.Value.Date < today && t.Status != TaskStatus.Completed)
            .OrderBy(t => t).ToList();
        return overdue;
    }

    public IReadOnlyList<TaskItem> Search(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return Array.Empty<TaskItem>();

        string word = keyword.Trim();
        StringComparison comparison = StringComparison.OrdinalIgnoreCase;

        List<TaskItem> results = _tasks
            .Where(t => t.Title.Contains(word, comparison) || t.Description.Contains(word, comparison))
            .OrderBy(t => t)
            .ToList();

        return results;
    }

    public IReadOnlyList<TaskItem> Filter(TaskFlow.Models.TaskStatus? status, TaskPriority? priority)
    {
        IEnumerable<TaskItem> query = _tasks;

        if (status.HasValue)
            query = query.Where(t => t.Status == status.Value);

        if (priority.HasValue)
        {
            query = query.Where(t => t.Priority == priority.Value);
        }

        List<TaskItem> results = query
            .OrderBy(t => t)
            .ToList();

        return results;
    }
    
    
}