using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
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
        private readonly IWebHostEnvironment _hostingEnvironment;

        public PointOfSaleRepository(PointOfSaleDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            db = context;
            _hostingEnvironment = hostingEnvironment;
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

                db.tbl_cart.RemoveRange(cart);
                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeductItem(int InvoiceId)
        {
            var cart = db.tbl_invoice.Where(x => x.Id == InvoiceId).SingleOrDefault();
            try
            {
                var sales = db.tbl_sales.Where(x => x.InvoiceId == InvoiceId).ToList();
                foreach (var item in sales)
                {
                    var totalQty = GetAvailableItem(item.Quantity, item.ProductId);

                    var product = db.tbl_item.Where(x => x.Id == item.ProductId).SingleOrDefault();
                    //product.Quantity = totalQty;
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public decimal GetTotalAmount(int InvoiceId)
        {
            var TotalAmount = 0.00M;
            var invoice = db.tbl_invoice.Where(x => x.Id == InvoiceId).SingleOrDefault();
            TotalAmount = invoice.AmountTotal;

            return TotalAmount;
        }

        public int GetAvailableItem(int Quantity, int ProductId)
        {
            var TotalAmount = 0;
            var product = db.tbl_item.Where(x => x.Id == ProductId).SingleOrDefault();
            //TotalAmount = product.Quantity - Quantity;

            return TotalAmount;
        }

        public FileStreamResult GenerateReport()
        {
            // Create a new PDF document
            iTextSharp.text.Document document = new iTextSharp.text.Document();

            // Set the output file path
            string relativePath = "reports/report.pdf";
            relativePath = relativePath.Replace("\\", "/");
            string outputPath = Path.Combine(_hostingEnvironment.WebRootPath, relativePath);

            // Create a new PDF writer
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(outputPath, FileMode.Create));

            // Open the PDF document
            document.Open();

            // Add content to the PDF document
            document.Add(new Paragraph("Hello, World!"));

            // Close the PDF document
            document.Close();

            MemoryStream stream = new MemoryStream();
            byte[] bytes = stream.ToArray();
            stream.Write(bytes, 0, bytes.Length);
            stream.Position = 0;

            //return outputPath;

            return new FileStreamResult(stream,"application/pdf");
        }
    }
}
