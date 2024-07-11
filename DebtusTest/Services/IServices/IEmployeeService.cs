using DebtusTest.Model;
using DebtusTest.Model.DTO;

namespace DebtusTest.Services.IServices
{
    public interface IEmployeeService
    {
        Task<EmployeeResponseDto> CreateEmployeeAsync(EmployeeCreateRequestDto request);
        Task<EmployeeResponseDto> UpdateEmployeeAsync(EmployeeUpdateRequestDto request);
        Task<EmptyResponseDto> DeleteEmployeeAsync(EmployeeIdRequestDto request);
        Task<List<EmployeeResponseDto>> GetEmployeesAsync(EmployeesByPositionRequestDto? request = null);
        Task<PositionsResponseDto> GetAllPositionsAsync();
    }
}
