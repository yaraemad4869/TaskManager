using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.Models;
using TaskManager.Core.Interfaces;
using TaskManager.Data;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;

        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            Users = new BasicRepo<User>(_db);
            UsersRepo = new UserRepo(_db);
            Categories = new BasicRepo<Category>(_db);
            CategoriesRepo = new CategoryRepo<Category>(_db);
            Colors = new BasicRepo<Color>(_db);
            ColorsRepo = new CategoryRepo<Color>(_db);
            //Items = new CategoryRepo<Item>(_db);
            //ItemsRepo = new ItemRepo<Item>(_db);
            ToDoLists = new CategoryRepo<ToDoList>(_db);
            ToDoesCRUD = new BasicRepo<ToDo>(_db);
            ToDoesRepo = new ToDoRepo(_db);
            ToDoes = new CategoryRepo<ToDo>(_db);
            Notes = new CategoryRepo<Note>(_db);
            NotesRepo = new ItemRepo<Note>(_db);
            ToDoListsRepo = new ItemRepo<ToDoList>(_db);
            ToDoListsCRUD = new BasicRepo<ToDoList>(_db);
            NotesCRUD = new BasicRepo<Note>(_db);
        }
        public IBasicRepo<User> Users { get; private set; }
        public IUserRepo UsersRepo { get; private set; }
        public IBasicRepo<Category> Categories { get; private set; }
        public ICategoryRepo<Category> CategoriesRepo { get; private set; }
        public IBasicRepo<Color> Colors { get; private set; }
        public ICategoryRepo<Color> ColorsRepo { get; private set; }
        public IBasicRepo<ToDoList> ToDoListsCRUD { get; private set; }
        public IItemRepo<Note> NotesRepo { get; private set; }
        public IBasicRepo<Note> NotesCRUD { get; }
        public IItemRepo<ToDoList> ToDoListsRepo { get; private set; }
        public ICategoryRepo<ToDoList> ToDoLists { get; private set; }
        public IToDoRepo ToDoesRepo { get; }
        public IBasicRepo<ToDo> ToDoesCRUD { get; }
        public ICategoryRepo<ToDo> ToDoes { get; }
        public ICategoryRepo<Note> Notes { get; private set; }
        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
