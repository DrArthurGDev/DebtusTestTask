using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Shift
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public double? HoursWorked { get; set; }

    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }

    public Employee Employee { get; set; }
}