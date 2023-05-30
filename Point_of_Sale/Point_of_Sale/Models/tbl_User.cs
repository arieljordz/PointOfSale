using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Point_of_Sale.Models
{
    public class tbl_User
    {
        [Key]
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        public string? FullName { get; set; }

        public int UsertypeId { get; set; }

        public string? UserName { get; set; }

        public string? Password { get; set; }

        public DateTime DateRegistered { get; set; }
    }
}
