using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DebtusTest.Model;


public class Employee
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }
    [MaxLength(50)]
    public string MiddleName { get; set; }

    [Required]
    [ForeignKey("Position")]
    public int PositionId { get; set; }
    
    public Position Position { get; set; }
    
    public List<Shift> Shifts { get; set; } = new List<Shift>();
}