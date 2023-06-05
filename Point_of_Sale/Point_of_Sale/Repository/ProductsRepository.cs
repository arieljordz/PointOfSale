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

        public List<ProductsDTO> GetItemDetails()
        {
            List<ProductsDTO> objList = new List<ProductsDTO>();


            //var result = (from a in db.tbl_item
            //             join b in db.tbl_itemDetails on a.Id equals b.ProductId
            //             join c in db.tbl_brand on a.BrandId equals c.Id
            //             join d in db.tbl_supplier on a.SupplierId equals d.Id
            //             group b by a.Description into g
            //             orderby g.Max(a => a.Description)
            //             select new
            //             {
            //                 ProductId = g.Max(a => a.Id),
            //                 Description = g.Key,
            //                 Brand = g.Max(a => c.Description),
            //                 Supplier = g.Max(a => d.Description),
            //                 Quantity = g.Sum(a => a.Quantity),
            //                 Price = g.Max(a => a.Price),
            //                 DateAdded = g.Max(a => a.DateAdded.ToString("MM/dd/yyyy")),
            //                 DateExpired = g.Max(a => a.DateExpired.ToString("MM/dd/yyyy"))
            //             }).ToList();

            var list = (from a in db.tbl_item
                        join b in db.tbl_itemDetails on a.Id equals b.ProductId
                        join c in db.tbl_brand on a.BrandId equals c.Id
                        join d in db.tbl_supplier on a.SupplierId equals d.Id
                        select new { a, b, c, d }).ToList();

            foreach (var item in list)
            {
                ProductsDTO obj = new ProductsDTO()
                {
                    ProductId = item.a.Id,
                    Description = item.a.Description,
                    Brand = item.c.Description,
                    Supplier = item.d.Description,
                    Quantity = item.b.Quantity,
                    Price = item.b.Price,
                    DateAdded = item.b.DateAdded.ToShortDateString(),
                    DateExpired = item.b.DateExpired.ToShortDateString(),
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
