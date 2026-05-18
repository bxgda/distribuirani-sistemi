using Grpc.Core;

/*
 
    1. U .NET-u, koristeći gRPC, kreirati servis za upravljanje listom zadataka. Servis treba da
    omogući klijentima da dodaju zadatke, prikažu sve zadatke i označe zadatke kao završene.
    Zahtevi:
        • Definisati proto fajl (tasks.proto) koji daje specifikaciju servisa i poruka. Servis treba da podržava sledeće operacije:
        • AddTask: Dodaje novi zadatak na listu.
        • ListTasks: Dobija listu svih zadataka.
        • MarkTaskAsCompleted: Označava zadatak kao završen po njegovom ID-u.
        • Implementirati gRPC server definisan u tasks.proto fajlu

 */

/*
    grpcurl -plaintext -d '{\"tekstZadatka\": \"Nauciti gRPC\"}' localhost:5110 tasks.Tasks/AddTask
    grpcurl -plaintext localhost:5110 tasks.Tasks/ListTasks
    grpcurl -plaintext -d '{\"id\": 1}' localhost:5110 tasks.Tasks/MarkTaskAsCompleted
*/

// NE MORA lock-ovi da se koriste ipak, nego to nisam znao kad sam radio ovaj zadatak
// tako da ispada jos prostije onda

namespace grpc2026.Services;

public class TaskService : Tasks.TasksBase
{
    private static readonly List<Zadatak> _tasks = new();
    private static int _nextId = 1;
    private static readonly object _lock = new();

    public override Task<TaskReply> AddTask(ZadatakBezId request, ServerCallContext context)
    {
        lock (_lock)
        {
            var zadatak = new Zadatak
            {
                Id = _nextId++,
                TekstZadatka = request.TekstZadatka,
                Completed = false,
            };
            _tasks.Add(zadatak);
            return Task.FromResult(new TaskReply { Message = "Task added successfully!" });
        }
    }

    public override Task<TaskList> ListTasks(Empty request, ServerCallContext context)
    {
        var taskList = new TaskList();
        taskList.Tasks.AddRange(_tasks);
        return Task.FromResult(taskList);
    }

    public override Task<TaskReply> MarkTaskAsCompleted(TaskId request, ServerCallContext context)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == request.Id);
        if (task == null)
        {
            return Task.FromResult(new TaskReply { Message = "Task not found!" });
        }
        lock (_lock)
        {
            task.Completed = true;
            return Task.FromResult(new TaskReply { Message = "Task marked as completed!" });
        }
    }
}
