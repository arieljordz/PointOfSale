using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Point_of_Sale.Models
{
    [Keyless]
    public class sp_get_items
    {
        public int ProductId { get; set; }

        public string? Description { get; set; }

        public string? Brand { get; set; }

        public string? Supplier { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public string? DateAdded { get; set; }

        public string? DateExpired { get; set; }

    }
}
