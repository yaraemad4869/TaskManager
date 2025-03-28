﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Models;

namespace TaskManager.IRepositories
{
    public interface IUnitOfWork
    {
        IBasicRepo<User> Users { get; }
        IUserRepo UsersRepo { get; }
        IBasicRepo<Category> Categories {  get; }
        ICategoryRepo CategoriesRepo { get; }
        IBasicRepo<Color> Colors { get; }
        IBasicRepo<Item> Items { get; }
        IBasicRepo<ToDoList> ToDoLists { get; }
        IBasicRepo<ToDo> ToDos { get; }
        IBasicRepo<Note> Notes { get; }
        IItemRepo<Note> NotesRepo { get; }
        IItemRepo<ToDoList> ToDoListsRepo { get; }
    }
}
