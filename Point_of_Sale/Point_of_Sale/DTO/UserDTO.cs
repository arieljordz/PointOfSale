using Microsoft.AspNetCore.Mvc.Rendering;
using Point_of_Sale.Models;

namespace Point_of_Sale.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        public string? FullName { get; set; }

        public int UsertypeId { get; set; }

        public string? UserName { get; set; }

        public string? Password { get; set; }

        public DateTime DateRegistered { get; set; }

        public SelectList UserTypeList { get; set; }
    }
}
