using System.ComponentModel.DataAnnotations;

namespace DebtusTest.Model.DTO;

public class EmployeeUpdateRequestDto
{
    [Required]
    public int Id { get; set; }

    [StringLength(50, ErrorMessage = "LastName must contain no more than {1} characters")]
    public string? LastName { get; set; }

    
    [StringLength(50, ErrorMessage = "LastName must contain no more than {1} characters")]
    public string? FirstName { get; set; }
    
    [StringLength(50, ErrorMessage = "LastName must contain no more than {1} characters")]
    public string? MiddleName { get; set; }
    
    public int PositionId { get; set; }
}