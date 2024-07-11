

namespace DebtusTest.Model.DTO;

public class EmployeeResponseDto
{
    public int Id { get; set; }

    public string LastName { get; set; }

    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    public int PositionId { get; set; }
    
    public ViolationResponseDto? ShiftsState { get; set; }
}