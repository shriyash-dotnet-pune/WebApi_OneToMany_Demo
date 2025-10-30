﻿using System.ComponentModel.DataAnnotations;

namespace WebApi_OneToMany_Demo.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
