using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace TaskManager.Data.Models
{
    public class Note : Item
    {
        public string Description { get; set; }
    }
}
