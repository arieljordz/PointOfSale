using AspNetCore.ReportingServices.ReportProcessing.ReportObjectModel;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Point_of_Sale.DTO;
using Point_of_Sale.Interface;
using Point_of_Sale.Models;
using Point_of_Sale.Models.DBContext;
using static iTextSharp.text.pdf.AcroFields;

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
            var list = global.GetItems();

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
            List<object> data = new List<object>();

            var _list = (from a in db.tbl_item
                         join b in db.tbl_brand
                         on a.BrandId equals b.Id
                         join c in db.tbl_supplier
                         on a.SupplierId equals c.Id
                         select new
                         {
                             ProductId = a.Id,
                             Description = a.Description,
                             Brand = b.Description,
                             Supplier = c.Description,
                             Quantity = a.Quantity,
                             Price = a.Price.ToString(),
                             DateExpired = a.DateExpired,
                         }).ToList();

            if (_list != null)
            {
                foreach (var item in _list)
                {
                    var obj = new
                    {
                        Id = item.ProductId,
                        Description = item.Description,
                        Brand = item.Brand == null ? "" : item.Brand,
                        Supplier = item.Supplier == null ? "" : item.Supplier,
                        Quantity = item.Quantity == null ? 0 : item.Quantity,
                        Price = item.Price == null ? "0.00" : item.Price,
                        DateAdded = db.tbl_itemDetails.Where(x => x.ProductId == item.ProductId).ToList().Max(x=>x.DateAdded).ToShortDateString(),
                        DateExpired = db.tbl_itemDetails.Where(x => x.ProductId == item.ProductId).ToList().Max(x => x.DateExpired).ToShortDateString(),
                    };
                    data.Add(obj);
                }
                return Json(new { data = data });
            }
            else
            {
                return Json(new { data = data });
            }
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
