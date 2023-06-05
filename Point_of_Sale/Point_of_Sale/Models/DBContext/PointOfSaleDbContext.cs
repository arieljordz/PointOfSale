using Microsoft.EntityFrameworkCore;
namespace Point_of_Sale.Models.DBContext
{
    public class PointOfSaleDbContext : DbContext
    {
        public PointOfSaleDbContext(DbContextOptions<PointOfSaleDbContext> options) : base(options) { }

        public DbSet<tbl_Bank> tbl_bank { get; set; }
        public DbSet<tbl_Invoice> tbl_invoice { get; set; }
        public DbSet<tbl_Item> tbl_item { get; set; }
        public DbSet<tbl_ItemDetails> tbl_itemDetails { get; set; }
        public DbSet<tbl_PaymentType> tbl_paymentType { get; set; }
        public DbSet<tbl_Sales> tbl_sales { get; set; }
        public DbSet<tbl_User> tbl_user { get; set; }
        public DbSet<tbl_UserType> tbl_userType { get; set; }
        public DbSet<tbl_Cart> tbl_cart { get; set; }
        public DbSet<tbl_Inventory> tbl_inventory { get; set; }
        public DbSet<tbl_Brand> tbl_brand { get; set; }
        public DbSet<tbl_Supplier> tbl_supplier { get; set; }
        public DbSet<sp_receipt> sp_receipt { get; set; }
        public DbSet<sp_generated_list> sp_generated_list { get; set; }

    }
}
