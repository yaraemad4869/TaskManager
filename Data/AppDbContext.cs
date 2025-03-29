using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManager.Core.Models;

namespace TaskManager.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the composite unique constraint
            modelBuilder.Entity<Category>()
                .HasIndex(c => new { c.UserId, c.Name })
                .IsUnique();
            modelBuilder.Entity<ToDo>()
                .HasIndex(t => new { t.ToDoListId, t.Name })
                .IsUnique();
            modelBuilder.Entity<Color>()
                .HasIndex(c => new { c.UserId, c.Name })
                .IsUnique();
            modelBuilder.Entity<Item>()
                .HasIndex(t => new { t.UserId, t.Title })
                .IsUnique();
            modelBuilder.Entity<Item>()
                .HasDiscriminator<string>("ItemType")
                .HasValue<Note>("Note")
                .HasValue<ToDoList>("ToDoList");
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique();
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<ToDoList> toDoLists { get; set; }
        public DbSet<ToDo> ToDos { get; set; }

    }
}
