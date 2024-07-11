using System.Net;
using DebtusTest.Data;
using DebtusTest.Model;
using DebtusTest.Model.DTO;
using DebtusTest.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DebtusTest.Controller;


[ApiController]
[Route("api/checkpoint")]
public class CheckpointController : ControllerBase
{
    private readonly IShiftService _shiftService;

    public CheckpointController(IShiftService shiftService)
    {
        _shiftService = shiftService;
    }

    [HttpPost("startshift")]
    public async Task<ApiResponse<EmptyResponseDto>> StartShift([FromBody] ShiftRequestDto request)
    {
        var result = await _shiftService.StartShiftAsync(request);
        return new ApiResponse<EmptyResponseDto>()
        {
            StatusCode = HttpStatusCode.OK,
            Result = new EmptyResponseDto()
        };
    }

    [HttpPost("endshift")]
    public async Task<ApiResponse<EmptyResponseDto>> EndShift([FromBody] ShiftRequestDto request)
    {
        var result = await _shiftService.EndShiftAsync(request);
        return new ApiResponse<EmptyResponseDto>()
        {
            StatusCode = HttpStatusCode.OK,
            Result = new EmptyResponseDto()
        };
    }
}