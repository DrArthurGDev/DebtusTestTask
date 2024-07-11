using System.Net;
using DebtusTest.Model;
using DebtusTest.Model.DTO;
using DebtusTest.Services.IServices;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/hr")]
public class HrController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public HrController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpPost("employee")]
    public async Task<ApiResponse<EmployeeResponseDto>> CreateEmployee([FromBody] EmployeeCreateRequestDto request)
    {
        var result = await _employeeService.CreateEmployeeAsync(request);
        return new ApiResponse<EmployeeResponseDto>
        {
            StatusCode = HttpStatusCode.OK,
            Result = result
        };
    }

    [HttpPut("employee")]
    public async Task<ApiResponse<EmployeeResponseDto>> UpdateEmployee([FromBody] EmployeeUpdateRequestDto request)
    {
        var result = await _employeeService.UpdateEmployeeAsync(request);
        return new ApiResponse<EmployeeResponseDto>
        {
            StatusCode = HttpStatusCode.OK,
            Result = result
        };
    }

    [HttpDelete("employee")]
    public async Task<ApiResponse<EmptyResponseDto>> DeleteEmployee([FromQuery] EmployeeIdRequestDto request)
    {
        var emptyResult = await _employeeService.DeleteEmployeeAsync(request);
        return new ApiResponse<EmptyResponseDto>
        {
            StatusCode = HttpStatusCode.OK,
            Result = emptyResult
        };
    }

    [HttpGet("employees")]
    public async Task<ApiResponse<List<EmployeeResponseDto>>> GetEmployees([FromQuery] EmployeesByPositionRequestDto? position = null)
    {
        var employeeResult = await _employeeService.GetEmployeesAsync(position);
        return new ApiResponse<List<EmployeeResponseDto>> 
        {
            StatusCode = HttpStatusCode.OK,
            Result = employeeResult
        };
    }

    [HttpGet("positions")]
    public async Task<ApiResponse<PositionsResponseDto>> GetAllPositions()
    {
        var result = await _employeeService.GetAllPositionsAsync();
        return new ApiResponse<PositionsResponseDto>
        {
            StatusCode = HttpStatusCode.OK,
            Result = result
        };
    }
}