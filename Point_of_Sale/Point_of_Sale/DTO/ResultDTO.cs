using Microsoft.AspNetCore.Mvc.Rendering;
using Point_of_Sale.Models;

namespace Point_of_Sale.DTO
{
    public class ResultDTO
    {
        public bool IsSuccess { get; set; }

        public string? Message { get; set; }
    }
}
