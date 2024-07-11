namespace DebtusTest.Model.DTO
{
    public class ShiftResponseDto
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double? HoursWorked { get; set; }
        public int EmployeeId { get; set; }
    }
}
