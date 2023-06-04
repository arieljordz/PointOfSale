using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Point_of_Sale.Models
{
    [Keyless]
    public class sp_generated_list
    {
        public string? InvoiceId { get; set; }

        public string? Description { get; set; }

        public string? Brand { get; set; }

        public string? Supplier { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal SubTotal { get; set; }

        public string? DateInvoiced { get; set; }
    }
}
