using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Models
{
    public class tbl_Brand
    {
        [Key]
        public int Id { get; set; }

        public string? Description { get; set; }
    }
}
