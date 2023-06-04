using Microsoft.AspNetCore.Mvc;
using Point_of_Sale.Interface;
using Point_of_Sale.Models.DBContext;
using System.Globalization;

namespace Point_of_Sale.Controllers
{
    public class InventoryController : Controller
    {
        private readonly PointOfSaleDbContext db;
        private readonly IGlobal global;

        public InventoryController(PointOfSaleDbContext context, IGlobal global_rep)
        {
            db = context;
            global = global_rep;
        }
        public IActionResult LoadViews()
        {
            ViewBag.DateNow = DateTime.Now;
            ViewBag.Username = Request.Cookies["FullName"];
            ViewBag.UserId = Request.Cookies["UserId"];
            ViewBag.UserType = Request.Cookies["UserType"];

            return View();
        }

        public IActionResult Inventory()
        {
            return LoadViews();
        }

        public IActionResult LoadItems()
        {
            var list = db.tbl_item.ToList();
            List<object> data = new List<object>();
            foreach (var item in list)
            {
                var obj = new
                {
                    Id = item.Id,
                    Description = item.Description,
                    Brand = item.Brand,
                    Supplier = item.Supplier,
                    Quantity = item.Quantity,
                    Price = item.Price.ToString(),
                    DateAdded = global.FormatDateMMDDYYYY(item.DateAdded.ToShortDateString()),
                    DateExpired = global.FormatDateMMDDYYYY(item.DateExpired.ToShortDateString()),
                };
                data.Add(obj);
            }
            return Json(new { data = data });
        }

        public IActionResult LoadSales()
        {
            var list = db.tbl_invoice.ToList();
            List<object> data = new List<object>();
            int Counter = 0;
            foreach (var item in list)
            {
                var obj = new
                {
                    Id = item.Id,
                    AmountTotal = item.AmountTotal.ToString(),
                    OrderNumber = "000" + item.Id.ToString(),
                    AccountNumber = item.AccountNumber,
                    DateInvoiced = global.FormatDateMMDDYYYY(item.DateInvoiced.ToShortDateString()),
                    Counter = Counter,
                };
                data.Add(obj);
                Counter++;
            }
            return Json(new { data = data });
        }
        public IActionResult LoadSalesChild(int InvoiceId)
        {
            var list = db.tbl_sales.Where(x => x.InvoiceId == InvoiceId).ToList();
            List<object> data = new List<object>();
            foreach (var item in list)
            {
                var prod = db.tbl_item.Where(x => x.Id == item.ProductId).SingleOrDefault();
                var obj = new
                {
                    Id = item.Id,
                    Description = prod != null ? prod.Description : "",
                    Quantity = item.Quantity,
                    SubTotal = item.SubTotal.ToString(),
                };
                data.Add(obj);
            }
            return Json(new { data = data });
        }

    }
}
