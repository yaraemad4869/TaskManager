using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
namespace TaskManager.Core.Models
{
    public class Category
    {
        public Category()
        {
            Items = new List<Item>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("user")]
        public int UserId { get; set; }
        public virtual User user { get; set; }
        public virtual ICollection<Item>? Items { get; set; }
    }
}
