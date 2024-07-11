using System.ComponentModel.DataAnnotations;

namespace DebtusTest.Model.DTO;

public class ShiftRequestDto
{
    [Required]
    public int EmployeeId{ get; set; }
        
    [Required]
    public DateTime RequestTime{ get; set; }
}