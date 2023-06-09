using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Point_of_Sale.Interface;
using Point_of_Sale.Models;
using Point_of_Sale.Models.DBContext;
using System.Globalization;
using AspNetCore.ReportingServices.ReportProcessing.ReportObjectModel;
using static iTextSharp.text.pdf.AcroFields;
using Point_of_Sale.DTO;
using System.Web.Helpers;

namespace Point_of_Sale.Repository
{
    public class PointOfSaleRepository : IPointOfSale
    {
        private readonly PointOfSaleDbContext db;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IGlobal global;

        public PointOfSaleRepository(PointOfSaleDbContext context, IWebHostEnvironment hostingEnvironment, IGlobal global_rep)
        {
            db = context;
            _hostingEnvironment = hostingEnvironment;
            global = global_rep;
        }

        public int SaveInvoice(int UserId)
        {
            int InvoiceId = 0;
            var cart = db.tbl_cart.Where(x => x.UserId == UserId).ToList();
            var qry = db.tbl_invoice.Where(x => x.UserId == UserId && x.IsPaid == false).SingleOrDefault();
            if (qry != null)
            {
                qry.PaymentTypeId = 1;
                qry.BankId = 1;
                qry.AccountNumber = "1234-567-890";
                qry.AmountTendered = 0.00M;
                qry.AmountTotal = GetTotalAmount(qry.Id) + cart.Sum(x => x.SubTotal);
                qry.DateInvoiced = DateTime.Now;
                db.SaveChanges();

                InvoiceId = qry.Id;
            }
            else
            {
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

                InvoiceId = invoice.Id;
            }
            return InvoiceId;
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
                if (cart != null)
                {
                    db.tbl_cart.RemoveRange(cart);
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ResultDTO DeductQuantity(int InvoiceId)
        {
            ResultDTO result = new ResultDTO();
            result.IsSuccess = true;
            result.Message = "Success";

            var sales = db.tbl_sales.Where(x => x.InvoiceId == InvoiceId).ToList();

            foreach (var item in sales)
            {
                var dtls = db.tbl_item.Where(x => x.Id == item.ProductId && x.Quantity >= item.Quantity).FirstOrDefault();
                if (dtls != null)
                {
                    dtls.Quantity = dtls.Quantity - item.Quantity;
                    db.SaveChanges();
                }
                else
                {
                    var itemDesc = db.tbl_item.Where(x => x.Id == item.ProductId).FirstOrDefault().Description;
                    if (itemDesc != null)
                    {
                        result.IsSuccess = false;
                        result.Message = "The available quantity of " + itemDesc + " is not enough!";
                        return result;
                    }
                }
            }
            return result;
        }
        public decimal GetTotalAmount(int InvoiceId)
        {
            var TotalAmount = 0.00M;
            var invoice = db.tbl_invoice.Where(x => x.Id == InvoiceId).SingleOrDefault();
            if (invoice != null)
            {
                TotalAmount = invoice.AmountTotal;
            }
            return TotalAmount;
        }

        public int GetAvailableItem(int Quantity, int ProductId)
        {
            var TotalQuantity = 0;
            var product = global.GetItemDetails().Where(x => x.ProductId == ProductId).SingleOrDefault();
            if (product != null)
            {
                TotalQuantity = product.Quantity - Quantity;
            }
            return TotalQuantity;
        }


    }
}
