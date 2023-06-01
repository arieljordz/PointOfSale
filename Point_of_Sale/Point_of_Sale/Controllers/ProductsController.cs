using Microsoft.AspNetCore.Mvc;
using Point_of_Sale.Interface;
using Point_of_Sale.Models;
using Point_of_Sale.Models.DBContext;

namespace Point_of_Sale.Controllers
{
    public class ProductsController : Controller
    {
        private readonly PointOfSaleDbContext db;
        private readonly IGlobal global;

        public ProductsController(PointOfSaleDbContext context, IGlobal global_rep)
        {
            db = context;
            global = global_rep;
        }
        public IActionResult LoadViews()
        {
            ViewBag.DateNow = DateTime.Now;
            ViewBag.Username = Request.Cookies["FullName"];
            ViewBag.UserId = Request.Cookies["UserId"];

            return View();
        }

        public IActionResult ProductDetails()
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

        [HttpPost]
        public IActionResult SaveItem(tbl_Item item)
        {
            try
            {
                if (item.Id != 0)
                {
                    var qry = db.tbl_item.Where(x => x.Id == item.Id).SingleOrDefault();
                    qry.Description = item.Description;
                    qry.Brand = item.Brand;
                    qry.Supplier = item.Supplier;
                    qry.Quantity = item.Quantity;
                    qry.Price = item.Price;
                    qry.DateExpired = item.DateExpired;
                    db.SaveChanges();
                }
                else
                {
                    item.DateAdded = DateTime.Now;
                    db.tbl_item.Add(item);
                    db.SaveChanges();

                }
                return Json(new { success = true });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }


        public IActionResult UpdateItem(int Id)
        {
            try
            {
                var data = db.tbl_item.Where(x => x.Id == Id).SingleOrDefault();
                return Json(new { data = data });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message });
            }
        }

        public IActionResult RemoveItem(int Id)
        {
            try
            {
                var data = db.tbl_item.Where(x => x.Id == Id).SingleOrDefault();
                db.tbl_item.Remove(data);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }



    }
}
