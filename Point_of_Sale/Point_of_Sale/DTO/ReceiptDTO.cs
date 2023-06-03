using Microsoft.AspNetCore.Mvc.Rendering;
using Point_of_Sale.Models;

namespace Point_of_Sale.DTO
{
    public class ReceiptDTO
    {
        public int InvoiceId { get; set; }

        public string? Description { get; set; }

        public int Quantity { get; set; }

        public decimal SubTotal { get; set; }
    }
}
