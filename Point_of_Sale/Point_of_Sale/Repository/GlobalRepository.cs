using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
     
        public decimal ComputePrice(int quantity, int productId)
        {
            decimal TotalPrice = 0.00M;
            var qry = db.tbl_item.Where(x => x.Id == productId).SingleOrDefault();
            if (qry != null)
            {
                TotalPrice = quantity * qry.Price;
            }
            return TotalPrice;
        }
        
        public decimal GetItemPrice(int productId)
        {
            decimal TotalPrice = 0.00M;
            var qry = db.tbl_item.Where(x => x.Id == productId).SingleOrDefault();
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
