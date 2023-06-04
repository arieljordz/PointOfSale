using AspNetCore.ReportingServices.ReportProcessing.ReportObjectModel;
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
        private readonly IProducts pro;

        public ProductsController(PointOfSaleDbContext context, IGlobal global_rep, IProducts pro_rep)
        {
            db = context;
            global = global_rep;
            pro = pro_rep;
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
                using (var ts = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (item.Id != 0)
                        {
                            // Update Item
                            tbl_Item? _Item = pro.SaveItem(item);
                        }
                        else
                        {
                            // Save Item
                            tbl_Item? _Item = pro.SaveItem(item);
                            if (_Item?.Id != 0)
                            {
                                // Save Item Details
                                bool Dtls = pro.SaveItemDetails(_Item);
                                if (!Dtls)
                                {
                                    ts.Rollback();
                                    ts.Dispose();
                                }
                            }
                        }
                        ts.Commit();
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
