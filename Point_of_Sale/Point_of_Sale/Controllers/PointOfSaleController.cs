using Microsoft.AspNetCore.Mvc;
using Point_of_Sale.DTO;
using Point_of_Sale.Interface;
using Point_of_Sale.Models;
using Point_of_Sale.Models.DBContext;
//using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using iTextSharp.text.pdf;
using iTextSharp.text;

namespace Point_of_Sale.Controllers
{
    public class PointOfSaleController : Controller
    {
        private readonly PointOfSaleDbContext db;
        private readonly IGlobal global;
        private readonly IPointOfSale pos;

        public PointOfSaleController(PointOfSaleDbContext context, IGlobal global_rep, IPointOfSale pos_rep)
        {
            db = context;
            global = global_rep;
            pos = pos_rep;
        }
        public IActionResult LoadViews()
        {
            ViewBag.DateNow = DateTime.Now;
            ViewBag.Username = HttpContext.Session.GetString("FullName");
            ViewBag.UserId = HttpContext.Session.GetString("UserId");
            ViewBag.UserType = HttpContext.Session.GetString("UserType");

            return View();
        }

        public IActionResult PointOfSale()
        {
            return LoadViews();
        }

        public IActionResult LoadCart()
        {
            var UserId = Convert.ToInt64(Request.Cookies["UserId"]);

            var list = db.tbl_cart.Where(x => x.UserId == UserId).ToList();

            List<object> data = new List<object>();
            foreach (var item in list)
            {
                var Description = db.tbl_item.Where(x => x.Id == item.ProductId).FirstOrDefault();
                var obj = new
                {
                    Id = item.Id,
                    Description = Description == null ? "" : Description.Description,
                    Quantity = item.Quantity,
                    Price = item.SubTotal.ToString(),
                };
                data.Add(obj);
            }
            return Json(new { data = data });
        }

        [HttpPost]
        public IActionResult AddToCart(tbl_Cart sales)
        {
            try
            {
                if (sales.Id != 0)
                {
                    var qry = db.tbl_cart.Where(x => x.Id == sales.Id).SingleOrDefault();
                    qry.Quantity = sales.Quantity;
                    qry.Price = global.GetItemPrice(sales.ProductId);
                    qry.SubTotal = global.ComputePrice(sales.Quantity, sales.ProductId);
                    db.SaveChanges();
                }
                else
                {
                    sales.Price = global.GetItemPrice(sales.ProductId);
                    sales.SubTotal = global.ComputePrice(sales.Quantity, sales.ProductId);
                    db.tbl_cart.Add(sales);
                    db.SaveChanges();

                }
                return Json(new { success = true });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }

        public IActionResult RemoveItem(int Id)
        {
            try
            {
                var data = db.tbl_cart.Where(x => x.Id == Id).SingleOrDefault();
                db.tbl_cart.Remove(data);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public IActionResult Checkout(int UserId)
        {
            try
            {
                ResultDTO result = new ResultDTO(); 

                var AmountTotal = 0.00M;

                using (var ts = db.Database.BeginTransaction())
                {
                    try
                    {
                        // Save to Invoice
                        int InvoiceId = pos.SaveInvoice(UserId);
                        if (InvoiceId != 0)
                        {
                            // Save to sales
                            bool Sales = pos.SaveSales(UserId, InvoiceId);
                            if (!Sales)
                            {
                                ts.Rollback();
                                ts.Dispose();
                            }
                            else
                            {
                                // Deduct Quantity
                                result = pos.DeductQuantity(InvoiceId);
                                if (!result.IsSuccess)
                                {
                                    ts.Rollback();
                                    ts.Dispose();
                                }
                                else
                                {
                                    ts.Commit();
                                }
                            }
                            AmountTotal = pos.GetTotalAmount(InvoiceId);
                        }
                    }
                    catch (Exception ex)
                    {
                        ts.Rollback();
                        ts.Dispose();
                    }
                }
                if (result.IsSuccess)
                {
                    return Json(new { success = true, TotalAmount = AmountTotal });
                }
                else
                {
                    return Json(new { success = false, message = result.Message });
                }
               
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public IActionResult GetBalance(CheckoutDTO dto)
        {
            try
            {
                decimal balance = 0.00M;
                int InvoiceId = 0;
                var invoice = db.tbl_invoice.Where(x => x.UserId == dto.UserId && x.IsPaid == false).SingleOrDefault();
                if (dto.AmountPaid != 0)
                {
                    if (invoice != null)
                    {
                        balance = dto.AmountPaid - invoice.AmountTotal;
                        invoice.IsPaid = true;
                        invoice.PaymentTypeId = 1;
                        invoice.BankId = 1;
                        invoice.AccountNumber = "1234-567-890";
                        invoice.AmountTendered = dto.AmountPaid;
                        invoice.DateInvoiced = DateTime.Now;
                        db.SaveChanges();
                    }
                    InvoiceId = invoice.Id;
                    return Json(new { success = true, balance = balance, InvoiceId = InvoiceId });
                }
                else
                {
                    return Json(new { success = false, message = "Please enter the Amount Paid." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public IActionResult GeneratePdf()
        {
            // Create a new PDF document
            Document document = new Document();

            // Create a new MemoryStream to write the PDF content
            MemoryStream memoryStream = new MemoryStream();

            // Create a PdfWriter instance to write the PDF document to the MemoryStream
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

            // Open the PDF document
            document.Open();

            // Add content to the PDF document
            document.Add(new Paragraph("Hello, World!"));

            // Flush and close the PdfWriter to ensure all content is written to the MemoryStream
            //writer.Flush();
            //writer.Close();

            // Set the position to the beginning of the MemoryStream
            memoryStream.Position = 0;

            // Return the PDF as a FileStreamResult
            return new FileStreamResult(memoryStream, "application/pdf");
        }
    }
}
