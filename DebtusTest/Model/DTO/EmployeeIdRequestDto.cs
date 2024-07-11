using System.ComponentModel.DataAnnotations;

namespace DebtusTest.Model.DTO;

public class EmployeeIdRequestDto
{
    [Required]
    public int Id { get; set; }
}