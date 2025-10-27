using System.ComponentModel.DataAnnotations;
using TaskFlow.Models;

var project = new Project("Demo Project");

project.AddTask(new PersonalTask("Buy groceries", "Egss, Milk, Flour", "Alexia", TaskPriority.Low, TaskFlow.Models.TaskStatus.Completed, new DueDate(DateTime.Parse("2025-11-05"))));
project.AddTask(new WorkTask("Work on project", "Research about something", "Razvan", TaskPriority.High, TaskFlow.Models.TaskStatus.InProgress,  new DueDate(DateTime.Parse("2025-10-28"))));

while (true)
{
    Console.WriteLine("\n === TaskFlow === ");
    Console.WriteLine("1. List tasks");
    Console.WriteLine("2. Add task");
    Console.WriteLine("3. Complete task");
    Console.WriteLine("4. List tasks which are overdue ");
    Console.WriteLine("5. Show all completed tasks");
    Console.WriteLine("6. Search tasks");
    Console.WriteLine("7. Filter task");
    Console.WriteLine("8. Exit");
    
    string option = Console.ReadLine();

    switch (option)
    {
        case "1": ListTasks(); break;
        case "2": AddTask(); break;
        case "3": CompleteTask(); break;
        case "4": ListOverdue(); break;
        case "5": CompleteAll(); break;
        case "6": SearchMenu(); break;
        case "7": FilterMenu(); break;
        case "8": Console.WriteLine("Exit"); return;
        default: Console.WriteLine("Unknown option"); break;
    }

    void ListTasks()
    {
        IReadOnlyList<TaskItem> task = project.ListTaskSorted();
        if (task.Count == 0)
        {
            Console.WriteLine("No tasks found");
            return;
        }
        
        Console.WriteLine($"\n Project: {project.Name}");

        PrintTasks(task);
    }

    void AddTask()
    {
        
        Console.Write("Type (Personal/Work): ");
        string taskType = (Console.ReadLine() ?? string.Empty).Trim().ToLowerInvariant();
        
        Console.Write("Title: ");
        
        string title = (Console.ReadLine() ?? string.Empty).Trim();

        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("Title is required");
            return;
        }
        
        Console.Write("Description: ");
        string description = (Console.ReadLine() ?? string.Empty).Trim();
        
        TaskPriority priority;
        while (true)
        {
            Console.Write("Priority: ");
            string? input = Console.ReadLine();
                    
            bool parsed = Enum.TryParse<TaskPriority>(input, true, out priority);

            if (parsed)
            {
                break;
            }
                    
            Console.WriteLine("Invalid input");
        }
        
        TaskFlow.Models.TaskStatus status;
        while (true)
        {
            Console.Write("Status (New/InProgress/Completed): ");
            string? input = Console.ReadLine();
                    
            bool parsed = Enum.TryParse<TaskFlow.Models.TaskStatus>(input, true, out status);

            if (parsed)
            {
                break;
            }
                    
            Console.WriteLine("Invalid input");
        }
        
        Console.Write("Due Date (yyyy-MM-dd): ");
        string? dateInput = Console.ReadLine();
        DateTime parsedDate;

        if (!DateTime.TryParse(dateInput, out parsedDate))
        {
            Console.WriteLine("Invalid date");
            return;
        }
        DueDate dueDate = new DueDate(parsedDate);
        
        switch (taskType)
        {
            case "personal":
                Console.Write("Person:");
                string? person = Console.ReadLine();
                PersonalTask personalTask = new PersonalTask(title, description, person, priority, status, dueDate);
                project.AddTask(personalTask);
                Console.WriteLine("Task added" + personalTask.Details());
                break;
            case "work":
                Console.Write("Assignee: ");
                string? assignee = Console.ReadLine();
                
                WorkTask workTask = new WorkTask(title, description, assignee, priority, status, dueDate);
                project.AddTask(workTask);
                Console.WriteLine("Task added" + workTask.Details());
                break;
            default:
            {
                Console.WriteLine("Unknown option");
                break;
            }
        }
        
    }

    void CompleteTask()
    {
        Console.Write("Task id: ");
        string? id = Console.ReadLine();
        if (!int.TryParse(id, out int idOut))
        {
            Console.WriteLine("Invalid id");
            return;
        }

        try
        {
            bool ok = project.CompleteTask(idOut);
            Console.WriteLine(ok ? "Task completed" : "Task not completed");
        }
        catch (TaskNotFoundException ex)
        {
            Console.WriteLine("[NOT FOUND]" +ex.Message);
        }
        catch (InvalidCastException ex)
        {
            Console.WriteLine("[INVALID]" +  ex.Message);
        }
    }
    
    void CompleteAll()
    {
        IReadOnlyList<TaskItem> completed = project.ListCompletedTask();

        if (completed.Count == 0)
        {
            Console.WriteLine("No tasks found");
            return;
        }
        
        Console.WriteLine($"\n {completed.Count} tasks completed");
        PrintTasks(completed);
    }

    void ListOverdue()
    {
        IReadOnlyList<TaskItem> overdue = project.ListOverdue();
        if (overdue.Count == 0)
        {
            Console.WriteLine("No tasks found");
        }
        Console.WriteLine("\n Overdue");
        
        PrintTasks(overdue);

    }

    void PrintTasks(IReadOnlyList<TaskItem> tasks)
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("No tasks found");
            return;
        }
        
        Console.WriteLine();
        Console.WriteLine(
            $"{ "Id",3}  { "Type",-12}  { "Title",-24}  { "Status",-11}  { "Priority",-7}  { "Due",-10}  { "Extra",-18}"
        );
        Console.WriteLine(new string('-', 3 + 2 + 12 + 2 + 24 + 2 + 11 + 2 + 7 + 2 + 10 + 2 + 18));
        
        DateTime today = DateTime.Now;

        foreach (TaskItem t in tasks)
        {
            string type = t.GetType().Name;
            string title =  t.Title;
            string due = t.DueDate.ToString();

            string extra =
                t is PersonalTask pt
                    ? $"For: {(string.IsNullOrWhiteSpace(pt.Person) ? "(self)" : pt.Person)}"
                    :
                    t is WorkTask wt
                        ?
                        $"Assignee: {(string.IsNullOrWhiteSpace(wt.Assignee) ? "(unassigned)" : wt.Assignee)}"
                        :
                        string.Empty;
            
            bool isOverdue = t.Status != TaskFlow.Models.TaskStatus.Completed && t.DueDate.Value.Date < today;
            
            ConsoleColor? previous = Console.ForegroundColor;
            
            if(isOverdue)
                Console.ForegroundColor = ConsoleColor.Red;
            else if (t.Priority == TaskPriority.High)
                Console.ForegroundColor = ConsoleColor.Yellow;
            else Console.ForegroundColor = ConsoleColor.Green;
            
            Console.WriteLine(
                $"{t.Id,3}  {type,-12}  {title,-24}  {t.Status,-11}  {t.Priority,-7}  {due,-10}  {extra,-18}"
            );

            Console.ForegroundColor = previous.Value;
        }
        
    }

    void SearchMenu()
    {
        Console.Write("Search: ");
        
        string? search = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(search))
        {
            Console.WriteLine("Keyword is required");
            return;
        }
        
        IReadOnlyList<TaskItem> results = project.Search(search);

        if (results.Count == 0)
        {
            Console.WriteLine("No tasks found");
            return;
        }

        PrintTasks(results);
    }

    void FilterMenu()
    {
        Console.Write("Filter status: ");
        string? stInput = Console.ReadLine();

        TaskFlow.Models.TaskStatus? status = null;
        
        if (!string.IsNullOrWhiteSpace(stInput))
        {
            stInput = stInput.Trim();
            TaskFlow.Models.TaskStatus stParsed;
            bool ok = Enum.TryParse<TaskFlow.Models.TaskStatus>(stInput, true, out stParsed);
            if (!ok)
            {
                Console.WriteLine("Invalid status");
                return;
            }
            
            status =  stParsed;
        }
        
        Console.Write("Filter priority: ");
        string? prInput = Console.ReadLine();
        TaskPriority? priority = null;
        if (!string.IsNullOrWhiteSpace(prInput))
        {
            prInput = prInput.Trim();
            TaskPriority prParsed;
            bool ok = Enum.TryParse<TaskPriority>(prInput, true, out prParsed);
            if (!ok)
            {
                Console.WriteLine("Invalid priority");
                return;
            }
            priority = prParsed;
        }
        
        IReadOnlyList<TaskItem> results = project.Filter(status, priority);
        if(results.Count == 0)
        {
            Console.WriteLine("No tasks found");
            return;
        }

        PrintTasks(results);
    }
}



