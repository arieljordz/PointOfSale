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
    public class ProductsRepository : IProducts
    {
        private readonly PointOfSaleDbContext db;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductsRepository(PointOfSaleDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            db = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public tbl_Item? SaveItem(tbl_Item item)
        {
            int itemId = 0;
            if (item.Id != 0)
            {
                var qry = db.tbl_item.Where(x => x.Id == item.Id).SingleOrDefault();
                if (qry != null)
                {
                    qry.Description = item.Description;
                    qry.Brand = item.Brand;
                    qry.Supplier = item.Supplier;
                    qry.Quantity = item.Quantity;
                    qry.Price = item.Price;
                    qry.DateExpired = item.DateExpired;
                    db.SaveChanges();
                }
                var dtls = db.tbl_inventory.Where(x => x.ProductId == item.Id && x.DateAdded == qry.DateAdded).SingleOrDefault();
                if (dtls != null)
                {
                    dtls.DateExpired = item.DateExpired;
                    db.SaveChanges();
                }
                item = qry;
            }
            else
            {
                item.DateAdded = DateTime.Now;
                db.tbl_item.Add(item);
                db.SaveChanges();
                itemId = item.Id;
            }
            return item;
        }

        public bool SaveItemDetails(tbl_Item item)
        {
            try
            {
                tbl_Inventory inventory = new tbl_Inventory();
                inventory.ProductId = item.Id;
                inventory.Quantity = item.Quantity;
                inventory.DateExpired = item.DateExpired;
                inventory.DateAdded = item.DateAdded;
                db.tbl_inventory.Add(inventory);
                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
