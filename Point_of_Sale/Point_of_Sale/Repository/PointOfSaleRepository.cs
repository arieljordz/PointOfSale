using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Point_of_Sale.Interface;
using Point_of_Sale.Models;
using Point_of_Sale.Models.DBContext;
using System.Globalization;

namespace Point_of_Sale.Repository
{
    public class PointOfSaleRepository : IPointOfSale
    {
        private readonly PointOfSaleDbContext db;

        public PointOfSaleRepository(PointOfSaleDbContext context)
        {
            db = context;
        }

        public int SaveInvoice(int UserId)
        {
            var cart = db.tbl_cart.Where(x => x.UserId == UserId).ToList();

            tbl_Invoice invoice = new tbl_Invoice();
            invoice.UserId = UserId;
            invoice.CustomerId = UserId;
            invoice.PaymentTypeId = 1;
            invoice.BankId = 1;
            invoice.AccountNumber = "1234-567-890";
            invoice.AmountTendered = 0.00M;
            invoice.AmountTotal = cart.Sum(x => x.SubTotal);
            invoice.DateInvoiced = DateTime.Now;
            db.tbl_invoice.Add(invoice);
            db.SaveChanges();

            return invoice.Id;
        }

        public bool SaveSales(int UserId, int InvoiceId)
        {
            var cart = db.tbl_cart.Where(x => x.UserId == UserId).ToList();
            try
            {
                foreach (var item in cart)
                {
                    tbl_Sales sales = new tbl_Sales();

                    sales.ProductId = item.ProductId;
                    sales.InvoiceId = InvoiceId;
                    sales.Quantity = item.Quantity;
                    sales.Price = item.Price;
                    sales.SubTotal = item.SubTotal;
                    db.tbl_sales.Add(sales);
                    db.SaveChanges();
                }

                db.tbl_cart.RemoveRange(cart);
                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
