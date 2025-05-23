﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Core.Models
{
    public class Color
    {
        public Color()
        {
            Items = new List<Item>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string HeaderGrade { get; set; } = "#FFF9C4";
        public string BodyGrade { get; set; } = "#FFFFFF";
        public string HeaderTextGrade { get; set; } = "#000000";
        public string BodyTextGrade { get; set; } = "#000000";
        [ForeignKey("user")]
        public int UserId { get; set; }
        public virtual User user { get; set; }
        public virtual ICollection<Item>? Items { get; set; }
    }
}
