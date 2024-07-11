using DebtusTest.Model;
using DebtusTest.Model.DTO;

namespace DebtusTest.Services.IServices
{
    public interface IShiftService
    {

        Task<EmptyResponseDto> StartShiftAsync(ShiftRequestDto shiftStartDto);
        Task<EmptyResponseDto> EndShiftAsync(ShiftRequestDto shiftEndRequestDto);

    }
}
