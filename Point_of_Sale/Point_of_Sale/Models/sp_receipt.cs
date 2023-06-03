using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Point_of_Sale.Models
{
    [Keyless]
    public class sp_receipt
    {
        public int InvoiceId { get; set; }

        public string? Description { get; set; }

        public int Quantity { get; set; }

        public decimal SubTotal { get; set; }
    }
}
