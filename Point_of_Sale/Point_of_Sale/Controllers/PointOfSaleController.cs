using Microsoft.AspNetCore.Mvc;
using Point_of_Sale.Interface;
using Point_of_Sale.Models;
using Point_of_Sale.Models.DBContext;

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
            ViewBag.Username = Request.Cookies["FullName"];
            ViewBag.UserId = Request.Cookies["UserId"];

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
                var Description = db.tbl_item.Where(x => x.Id == item.ProductId).FirstOrDefault().Description;
                var obj = new
                {
                    Id = item.Id,
                    Description = Description,
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
                var cart = db.tbl_cart.Where(x => x.UserId == UserId).ToList();

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
                            ts.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        ts.Rollback();
                        ts.Dispose();
                    }
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}
