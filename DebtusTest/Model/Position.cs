using System.ComponentModel.DataAnnotations;

namespace DebtusTest.Model;

public class Position
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string positionName { get; set; }
}