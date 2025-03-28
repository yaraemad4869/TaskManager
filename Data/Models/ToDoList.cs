using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Data.Models
{
    public class ToDoList : Item
    {
        public ToDoList() {
        ToDos = new List<ToDo>();
        }
        public ICollection<ToDo>? ToDos { get; set; }
    }
}
