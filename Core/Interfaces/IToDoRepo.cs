using TaskManager.Core.Models;

namespace TaskManager.Core.Interfaces
{
    public interface IToDoRepo
    {
        Task<List<ToDo>>? GetAllByToDoListId(int toDoListId, int userId);
        Task<bool> IsUnique(string name, int toDoListId, int userId);
    }
}
