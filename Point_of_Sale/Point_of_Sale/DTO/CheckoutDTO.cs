using Microsoft.AspNetCore.Mvc.Rendering;
using Point_of_Sale.Models;

namespace Point_of_Sale.DTO
{
    public class CheckoutDTO
    {
        public int UserId { get; set; }

        public decimal AmountPaid { get; set; }
    }
}
