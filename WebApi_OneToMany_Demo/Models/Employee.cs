using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi_OneToMany_Demo.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
        public int? DepartmentId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public virtual Department? Department { get; set; }
    }
}
