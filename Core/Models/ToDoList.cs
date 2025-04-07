using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.Models
{
    public class ToDoList : Item
    {
        public ToDoList()
        {
            ToDos = new List<ToDo>();
        }
        public virtual ICollection<ToDo>? ToDos { get; set; }
    }
}
