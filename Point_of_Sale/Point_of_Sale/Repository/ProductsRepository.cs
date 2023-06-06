using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Point_of_Sale.Interface;
using Point_of_Sale.Models;
using Point_of_Sale.Models.DBContext;
using System.Globalization;
using Point_of_Sale.DTO;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using NuGet.Protocol.Core.Types;

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

        public List<ProductsDTO> GetItems()
        {
            List<ProductsDTO> objList = new List<ProductsDTO>();

            var list = (from a in db.tbl_item
                        join b in db.tbl_brand on a.BrandId equals b.Id
                        join c in db.tbl_supplier on a.SupplierId equals c.Id
                        select new { a, b, c }).ToList();

            foreach (var item in list)
            {
                ProductsDTO obj = new ProductsDTO()
                {
                    ProductId = item.a.Id,
                    Description = item.a.Description,
                    Brand = item.b.Description,
                    Supplier = item.c.Description,
                };
                objList.Add(obj);
            }
            return objList;
        }

        public IEnumerable<sp_get_items> Exec_sp_get_items()
        {
            string storedProcedureName = "sp_get_items";
            return db.sp_get_items.FromSqlRaw<sp_get_items>(storedProcedureName);
        }

        public List<ProductsDTO> GetItemDetails()
        {
            List<ProductsDTO> objList = new List<ProductsDTO>();

            IEnumerable<sp_get_items> list = Exec_sp_get_items();

            foreach (var item in list)
            {
                ProductsDTO obj = new ProductsDTO()
                {
                    ProductId = item.ProductId,
                    Description = item.Description,
                    Brand = item.Brand,
                    Supplier = item.Supplier,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    DateAdded = item.DateAdded,
                    DateExpired = item.DateExpired,
                };
                objList.Add(obj);
            }
            return objList;
        }

        public bool SaveItem(tbl_Item item)
        {
            try
            {
                if (item.Id != 0)
                {
                    var qry = db.tbl_item.Where(x => x.Id == item.Id).SingleOrDefault();
                    if (qry != null)
                    {
                        qry.Description = item.Description;
                        qry.BrandId = item.BrandId;
                        qry.SupplierId = item.SupplierId;
                        db.SaveChanges();
                    }
                    return true;
                }
                else
                {
                    db.tbl_item.Add(item);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SaveItemDetails(tbl_ItemDetails item)
        {
            try
            {
                tbl_ItemDetails dtls = new tbl_ItemDetails();
                dtls.ProductId = item.Id;
                dtls.Quantity = item.Quantity;
                dtls.Price = item.Price;
                dtls.DateExpired = item.DateExpired;
                dtls.DateAdded = DateTime.Now;
                db.tbl_itemDetails.Add(dtls);
                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
