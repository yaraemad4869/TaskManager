using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Interfaces;
using TaskManager.Core.Models;
using TaskManager.Data;

namespace TaskManager.Infrastructure.Repositories
{
    public class ToDoRepo : IToDoRepo
    {
        private readonly AppDbContext _db;
        public ToDoRepo(AppDbContext db)
        {
            _db = db;
        }
        public async Task<List<ToDo>>? GetAllByToDoListId(int toDoListId, int userId)
        {
            List<ToDo>? toDos = await _db.ToDos.Where(td => td.ToDoListId == toDoListId && td.toDoList.UserId == userId).ToListAsync();
            if (toDos == null || !toDos.Any())
            {
                return null;
            }
            _db.ToDos.RemoveRange(toDos);
            await _db.SaveChangesAsync();
            return toDos;
        }
        public async Task<bool> IsUnique(string name, int toDoListId, int userId)
        {
            return await _db.ToDos.AnyAsync(td => td.toDoList.UserId == userId && td.ToDoListId==toDoListId && td.Name == name);

        }
    }
}
