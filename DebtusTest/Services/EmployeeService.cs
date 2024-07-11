using DebtusTest.Data;
using DebtusTest.Model.DTO;
using DebtusTest.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace DebtusTest.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly TestDbContext _context;

        public EmployeeService(TestDbContext context)
        {
            _context = context;
        }

        // Create a new employee in the database
        public async Task<EmployeeResponseDto> CreateEmployeeAsync(EmployeeCreateRequestDto request)
        {
            // Check if the provided position exists
            bool positionExists = await _context.Positions.AnyAsync(p => p.Id == request.PositionId);
            if (!positionExists)
            {
                throw new ArgumentException("The position listed does not exist in the database");
            }

            // Map request data to employee entity
            var employee = new Employee
            {
                LastName = request.LastName,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                PositionId = request.PositionId
            };

            // Add employee entity to the database
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            // Return the created employee data
            return new EmployeeResponseDto
            {
                Id = employee.Id,
                LastName = employee.LastName,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                PositionId = employee.PositionId
            };
        }

        // Update an existing employee's details
        public async Task<EmployeeResponseDto> UpdateEmployeeAsync(EmployeeUpdateRequestDto request)
        {
            // Find the employee by ID
            var employee = await _context.Employees.FindAsync(request.Id);
            if (employee == null)
            {
                throw new KeyNotFoundException("Employee not found");
            }

            // Check if the provided position exists
            if (await _context.Positions.FindAsync(request.PositionId) == null)
            {
                throw new KeyNotFoundException("Position not found");
            }

            // Update employee details if provided
            if (!string.IsNullOrWhiteSpace(request.LastName))
            {
                employee.LastName = request.LastName;
            }

            if (!string.IsNullOrWhiteSpace(request.FirstName))
            {
                employee.FirstName = request.FirstName;
            }

            if (!string.IsNullOrWhiteSpace(request.MiddleName))
            {
                employee.MiddleName = request.MiddleName;
            }

            if (request.PositionId > 0)
            {
                employee.PositionId = request.PositionId;
            }

            // Update employee entity in the database
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();

            // Return the updated employee data
            return new EmployeeResponseDto
            {
                Id = employee.Id,
                LastName = employee.LastName,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                PositionId = employee.PositionId
            };
        }

        // Delete an employee by ID
        public async Task<EmptyResponseDto> DeleteEmployeeAsync(EmployeeIdRequestDto request)
        {
            // Find the employee by ID
            var employee = await _context.Employees.FindAsync(request.Id);
            if (employee == null)
            {
                throw new KeyNotFoundException("Employee not found");
            }

            // Remove employee entity from the database
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            // Return an empty response indicating success
            return new EmptyResponseDto();
        }

        // Get employee details by ID
        public async Task<EmployeeResponseDto> GetEmployeeByIdAsync(EmployeeIdRequestDto request)
        {
            // Find the employee by ID
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == request.Id);
            if (employee == null)
            {
                throw new KeyNotFoundException("Employee not found");
            }

            // Return the found employee data
            return new EmployeeResponseDto
            {
                Id = employee.Id,
                LastName = employee.LastName,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                PositionId = employee.PositionId
            };
        }

        // Get a list of employees, optionally filtered by position ID
        public async Task<List<EmployeeResponseDto>> GetEmployeesAsync(EmployeesByPositionRequestDto? request = null)
        {
            // Check if the provided position exists
            if (await _context.Positions.FindAsync(request.PositionId) == null && request.PositionId != null)
            {
                throw new KeyNotFoundException("Position not found");
            }

            // Retrieve the list of employees with optional filtering
            var employees = request.PositionId == null
                ? await _context.Employees
                    .Include(e => e.Position)
                    .Include(e => e.Shifts.Where(s => s.EndTime.HasValue))
                    .ToListAsync()
                : await _context.Employees
                    .Include(e => e.Position)
                    .Where(e => e.PositionId == request.PositionId)
                    .Include(e => e.Shifts.Where(s => s.EndTime.HasValue))
                    .ToListAsync();

            if (employees == null)
            {
                throw new InvalidOperationException("Employees not found");
            }

            // Map employee entities to response DTOs and include violation statistics
            var tasks = employees.Select(async e =>
            {
                var violationStatistics = await GetViolationStatisticsAsync(e);

                return new EmployeeResponseDto
                {
                    Id = e.Id,
                    LastName = e.LastName,
                    FirstName = e.FirstName,
                    MiddleName = e.MiddleName,
                    PositionId = e.PositionId,
                    ShiftsState = violationStatistics
                };
            });

            var results = await Task.WhenAll(tasks);
            return results.ToList();
        }

        // Get a list of all positions
        public async Task<PositionsResponseDto> GetAllPositionsAsync()
        {
            return new PositionsResponseDto
            {
                Positions = await _context.Positions.ToListAsync()
            };
        }

        // Get violation statistics for an employee
        private async Task<ViolationResponseDto> GetViolationStatisticsAsync(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee), "RequestDto cannot be null.");
            }

            var lastCompletedShift = employee.Shifts
                .OrderByDescending(s => s.EndTime.Value)
                .FirstOrDefault();

            if (lastCompletedShift == null)
            {
                return new ViolationResponseDto
                {
                    MonthlyShiftsCount = 0,
                    MonthlyViolationCount = 0
                };
            }

            var startDate = lastCompletedShift.EndTime.Value.AddMonths(-1);
            var recentShifts = employee.Shifts
                .Where(s => s.EndTime.Value >= startDate)
                .ToList();

            int totalViolations = 0;

            // Calculate total violations based on shift times and position
            foreach (var shift in recentShifts)
            {
                if (shift.StartTime.TimeOfDay > new TimeSpan(9, 0, 0) ||
                    shift.EndTime?.TimeOfDay < new TimeSpan(18, 0, 0))
                {
                    totalViolations++;
                }
                else if (employee.Position.positionName == "Candle tester" &&
                         (shift.EndTime?.TimeOfDay < new TimeSpan(21, 0, 0) ||
                          shift.StartTime.TimeOfDay > new TimeSpan(9, 0, 0)))
                {
                    totalViolations++;
                }
            }

            return new ViolationResponseDto
            {
                MonthlyShiftsCount = recentShifts.Count,
                MonthlyViolationCount = totalViolations
            };
        }
    }

}
