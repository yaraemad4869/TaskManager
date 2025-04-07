using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Models;

namespace TaskManager.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IBasicRepo<User> Users { get; }
        IUserRepo UsersRepo { get; }
        IBasicRepo<Category> Categories { get; }

        ICategoryRepo<Category> CategoriesRepo { get; }
        IBasicRepo<Color> Colors { get; }
        ICategoryRepo<Color> ColorsRepo { get; }
        //IBasicRepo<Item> Items { get; }
        ICategoryRepo<ToDoList> ToDoLists { get; }
        IToDoRepo ToDoesRepo { get; }
        ICategoryRepo<ToDo> ToDoes { get; }
        IBasicRepo<ToDo> ToDoesCRUD { get; }
        ICategoryRepo<Note> Notes { get; }
        IItemRepo<Note> NotesRepo { get; }
        IBasicRepo<Note> NotesCRUD { get; }
        IItemRepo<ToDoList> ToDoListsRepo { get; }
        IBasicRepo<ToDoList> ToDoListsCRUD { get; }
        public void Dispose();
    }
}
