using System.ComponentModel.DataAnnotations;
using System.Net;

namespace DebtusTest.Model
{
    public class ApiResponse<T>
    {
        [Required]
        public HttpStatusCode StatusCode { get; set; }
        
        [Required]
        public T Result { get; set; }
    }
}
