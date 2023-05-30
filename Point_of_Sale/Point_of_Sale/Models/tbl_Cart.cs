using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Models
{
    public class tbl_Cart
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int UserId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal SubTotal { get; set; }
    }
}
