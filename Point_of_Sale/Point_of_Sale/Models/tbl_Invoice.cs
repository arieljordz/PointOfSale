using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Models
{
    public class tbl_Invoice
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int CustomerId { get; set; }

        public int PaymentTypeId { get; set; }

        public int BankId { get; set; }

        public string? AccountNumber { get; set; }

        public decimal AmountTendered { get; set; }

        public decimal AmountTotal { get; set; }

        public DateTime DateInvoiced { get; set; }
    }
}
