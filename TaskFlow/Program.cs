using TaskFlow.Models;

var project = new Project("Demo Project");

project.AddTask(new PersonalTask("Buy groceries", "Egss, Milk, Flour", person:"Alexia"));
project.AddTask(new WorkTask("Work on project", "Research about", assignee: "Razvan", priority: TaskPriority.High));

while (true)
{
    Console.WriteLine("\n === TaskFlow === ");
    Console.WriteLine("1. List tasks");
    Console.WriteLine("2. Add task");
    Console.WriteLine("3. Complete task");
    Console.WriteLine("4. Exit");
    
    string option = Console.ReadLine();

    switch (option)
    {
        case "1": ListTasks(); break;
        case "2": AddTask(); break;
        case "3": CompleteTask(); break;
        case "4": Console.WriteLine("Exit"); return;
        default: Console.WriteLine("Unknown option"); break;
    }

    void ListTasks()
    {
        IReadOnlyList<TaskItem> task = project.ListTask();
        if (task.Count == 0)
        {
            Console.WriteLine("No tasks found");
            return;
        }
        
        Console.WriteLine($"\n Project: {project.Name}");

        foreach (TaskItem taskItem in task)
        {
            Console.WriteLine(taskItem.Details());
        }
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

        switch (taskType)
        {
            case "personal":
                Console.Write("Person:");
                string? person = Console.ReadLine();
                PersonalTask personalTask = new PersonalTask(title, description, person);
                project.AddTask(personalTask);
                Console.WriteLine("Task added" + personalTask.Details());
                break;
            case "work":
                Console.Write("Assignee: ");
                string? assignee = Console.ReadLine();

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
                
                WorkTask workTask = new WorkTask(title, description, assignee, priority);
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

        bool ok = project.CompleteTask(idOut);
        Console.WriteLine(ok ? "Task completed" : "Task not completed");
    }
}



