using DebtusTest.Data;
using DebtusTest.Model.DTO;
using DebtusTest.Model;
using DebtusTest.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace DebtusTest.Services
{
    public class ShiftService : IShiftService
    {
        private readonly TestDbContext _context;

        public ShiftService(TestDbContext context)
        {
            _context = context;
        }

        // Starts a new shift for the specified employee
        public async Task<EmptyResponseDto> StartShiftAsync(ShiftRequestDto request)
        {
            // Retrieve the employee from the database by ID
            var employee = await _context.Employees.FindAsync(request.EmployeeId);

            if (employee == null)
            {
                throw new KeyNotFoundException("Employee not found");
            }

            if (await _context.Shifts.AnyAsync(s => s.EmployeeId == request.EmployeeId && !s.EndTime.HasValue))
            {
                throw new InvalidOperationException("Employee already has an open shift");
            }

            // Create a new shift with the specified start time
            var shift = new Shift
            {
                EmployeeId = request.EmployeeId,
                StartTime = request.RequestTime
            };

            // Add the new shift to the database
            _context.Shifts.Add(shift);
            await _context.SaveChangesAsync();

            // Return an empty response indicating success
            return new EmptyResponseDto();
        }

        //Ends the current open shift for the specified employee
        public async Task<EmptyResponseDto> EndShiftAsync(ShiftRequestDto request)
        {
            // Retrieve the employee from the database by ID
            var employee = await _context.Employees.FindAsync(request.EmployeeId);

            if (employee == null)
            {
                throw new KeyNotFoundException("Employee not found");
            }

            // Find the open shift for the employee
            var shift = await _context.Shifts.FirstOrDefaultAsync(s => s.EmployeeId == request.EmployeeId && !s.EndTime.HasValue);

            if (shift == null)
            {
                throw new InvalidOperationException("No open shift found for the employee");
            }

            shift.EndTime = request.RequestTime;
            shift.HoursWorked = (shift.EndTime - shift.StartTime)?.TotalHours;

            // Validate the calculated working hours
            if (shift.HoursWorked <= 0)
            {
                throw new InvalidOperationException("Incorrect dates: negative number of hours worked");
            }
            if (shift.HoursWorked >= 24)
            {
                throw new InvalidOperationException("Incorrect dates: working hours exceed 24 hours");
            }

            // Update the shift in the database
            _context.Shifts.Update(shift);
            await _context.SaveChangesAsync();

            // Return an empty response indicating success
            return new EmptyResponseDto();
        }
    }
}
