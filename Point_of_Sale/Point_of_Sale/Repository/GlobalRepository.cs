using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Point_of_Sale.DTO;
using Point_of_Sale.Interface;
using Point_of_Sale.Models;
using Point_of_Sale.Models.DBContext;
using System.Globalization;

namespace Point_of_Sale.Repository
{
    public class GlobalRepository : IGlobal
    {
        private readonly PointOfSaleDbContext db;

        public GlobalRepository(PointOfSaleDbContext context)
        {
            db = context;
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

            IEnumerable<sp_get_items> list = db.sp_get_items.FromSqlRaw<sp_get_items>("sp_get_items");

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

        public decimal ComputePrice(int quantity, int productId)
        {
            decimal TotalPrice = 0.00M;
            var qry = GetItemDetails().Where(x => x.ProductId == productId).SingleOrDefault();
            if (qry != null)
            {
                TotalPrice = quantity * qry.Price;
            }
            return TotalPrice;
        }
        
        public decimal GetItemPrice(int productId)
        {
            decimal TotalPrice = 0.00M;
            var qry = GetItemDetails().Where(x => x.ProductId == productId).SingleOrDefault();
            if (qry != null)
            {
                TotalPrice = qry.Price;
            }
            return TotalPrice;
        }      
        
        public string FormatDateMMDDYYYY(string date)
        {
            DateTime _date = DateTime.ParseExact(date, "M/d/yyyy", CultureInfo.InvariantCulture);
            string formattedDate = _date.ToString("MM/dd/yyyy");
            return formattedDate;
        }
    }
}
