using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Enums;
namespace TaskManager.Data.Models
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public TaskType TaskType { get; set; } = TaskType.Note;
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        [ForeignKey("user")]
        public int UserId { get; set; }
        public User user { get; set; }
        [ForeignKey("color")]
        public int ColorId { get; set; } = 0;
        public Color color { get; set; }
        [ForeignKey("category")]
        public int CategoryId { get; set; } = 0;
        public Category category { get; set; }
    }
}
