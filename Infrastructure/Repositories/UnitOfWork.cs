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
            CategoriesRepo = new CategoryRepo(_db);
            Colors = new BasicRepo<Color>(_db);
            Items = new BasicRepo<Item>(_db);
            ToDoLists = new BasicRepo<ToDoList>(_db);
            ToDos = new BasicRepo<ToDo>(_db);
            Notes = new BasicRepo<Note>(_db);
            NotesRepo = new ItemRepo<Note>(_db);
            ToDoListsRepo = new ItemRepo<ToDoList>(_db);

        }
        public IBasicRepo<User> Users { get; private set; }
        public IUserRepo UsersRepo { get; private set; }
        public IBasicRepo<Category> Categories { get; private set; }
        public ICategoryRepo CategoriesRepo { get; private set; }
        public IBasicRepo<Color> Colors { get; private set; }
        public IBasicRepo<Item> Items { get; private set; }
        public IItemRepo<Note> NotesRepo { get; private set; }
        public IItemRepo<ToDoList> ToDoListsRepo { get; private set; }
        public IBasicRepo<ToDoList> ToDoLists { get; private set; }
        public IBasicRepo<ToDo> ToDos { get; private set; }
        public IBasicRepo<Note> Notes { get; private set; }
        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
