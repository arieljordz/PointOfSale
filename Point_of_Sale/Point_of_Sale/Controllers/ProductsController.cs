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

            var _list = db.tbl_item.ToList();

            if (_list != null)
            {
                var ItemDetails = global.GetItemDetails();

                var _itemList = (from a in _list
                                 join b in ItemDetails on a.Id equals b.ProductId into joinedList
                                 from b in joinedList.DefaultIfEmpty()
                                 select new
                                 {
                                     ProductId = a.Id,
                                     Description = a.Description,
                                     Brand = b?.Brand,
                                     Supplier = b?.Supplier,
                                     Quantity = b?.Quantity,
                                     Price = b?.Price.ToString(),
                                     DateAdded = b?.DateAdded,
                                     DateExpired = b?.DateExpired,
                                 }).ToList();


                foreach (var item in _itemList)
                {
                    var obj = new
                    {
                        Id = item.ProductId,
                        Description = item.Description,
                        Brand = item.Brand == null ? "" : item.Brand,
                        Supplier = item.Supplier == null ? "" : item.Supplier,
                        Quantity = item.Quantity == null ? 0 : item.Quantity,
                        Price = item.Price == null ? "0.00" : item.Price,
                        DateAdded = item.DateAdded == null ? "" : item.DateAdded,
                        DateExpired = item.DateExpired == null ? "" : item.DateExpired,
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

        public IActionResult LoadItemDtlsss()
        {
            var list = global.GetItemDetails();

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
                    DateAdded = item.DateAdded,
                    DateExpired = item.DateExpired,
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
