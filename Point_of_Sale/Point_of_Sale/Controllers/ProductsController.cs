using AspNetCore.ReportingServices.ReportProcessing.ReportObjectModel;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Point_of_Sale.DTO;
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

        public IActionResult LoadItems()
        {
            var list = pro.GetItems();

            List<object> data = new List<object>();
            foreach (var item in list)
            {
                var obj = new
                {
                    Id = item.ProductId,
                    Description = item.Description,
                    Brand = item.Brand,
                    Supplier = item.Supplier,
                };
                data.Add(obj);
            }
            return Json(new { data = data });
        }

        public IActionResult LoadItemDtls()
        {
            var list = pro.GetItemDetails();

            List<object> data = new List<object>();

            if (list.Count() != 0)
            {
                foreach (var item in list)
                {
                    var obj = new
                    {
                        Id = item.ProductId,
                        Description = item.Description,
                        Brand = item.Brand,
                        Supplier = item.Supplier,
                        Quantity = item.Quantity,
                        Price = item.Price.ToString(),
                        DateAdded = global.FormatDateMMDDYYYY(item.DateAdded),
                        DateExpired = global.FormatDateMMDDYYYY(item.DateExpired),
                    };
                    data.Add(obj);
                }
                return Json(new { data = data });
            }
            else
            {
                var _list = db.tbl_item.ToList();

                foreach (var item in _list)
                {
                    var obj = new
                    {
                        Id = item.Id,
                        Description = item.Description,
                        Brand = db.tbl_brand.Where(x => x.Id == item.BrandId).FirstOrDefault().Description,
                        Supplier = db.tbl_supplier.Where(x => x.Id == item.SupplierId).FirstOrDefault().Description,
                        Quantity = 0,
                        Price = "0.00",
                        DateAdded = "",
                        DateExpired = "",
                    };
                    data.Add(obj);
                }
                return Json(new { data = data });
            }
        }

        public IActionResult LoadItemDtlsss()
        {
            var list = pro.GetItemDetails();

            List<object> data = new List<object>();
            foreach (var item in list)
            {
                var obj = new
                {
                    Id = item.ProductId,
                    Description = item.Description,
                    Brand = item.Brand,
                    Supplier = item.Supplier,
                    Quantity = item.Quantity,
                    Price = item.Price.ToString(),
                    DateAdded = global.FormatDateMMDDYYYY(item.DateAdded),
                    DateExpired = global.FormatDateMMDDYYYY(item.DateExpired),
                };
                data.Add(obj);
            }
            return Json(new { data = data });
        }


        [HttpPost]
        public IActionResult SaveItem(tbl_Item item)
        {
            bool result = pro.SaveItem(item);
            if (result)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Error in saving." });
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

        [HttpPost]
        public IActionResult SaveItemDetails(tbl_ItemDetails dtls)
        {
            bool result = pro.SaveItemDetails(dtls);
            if (result)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Error in saving." });
            }
        }

    }
}
