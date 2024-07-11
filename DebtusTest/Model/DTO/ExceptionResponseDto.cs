using System.ComponentModel.DataAnnotations;

namespace DebtusTest.Model.DTO
{
    public class ExceptionResponseDto
    {
        [Required]
        public int StatusCode { get; set; }
        
        public string? Message { get; set; }
    
    }
}
