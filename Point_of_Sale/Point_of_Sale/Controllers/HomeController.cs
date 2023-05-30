using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Point_of_Sale.Interface;
using Point_of_Sale.Models;
using Point_of_Sale.Models.DBContext;
using System.Diagnostics;

namespace Point_of_Sale.Controllers
{
    public class HomeController : Controller
    {
        private readonly PointOfSaleDbContext db;
        private readonly IGlobal global;

        public HomeController(PointOfSaleDbContext context, IGlobal global_rep)
        {
            db = context;
            global = global_rep;
        }


        public IActionResult LoadViews()
        {
            ViewBag.DateNow = DateTime.Now;
            ViewBag.Username = Request.Cookies["FullName"];
            ViewBag.UserId = Request.Cookies["UserId"];
            var userType = db.tbl_userType.ToList();
            ViewBag.cmbUserType = new SelectList(userType, "UsertypeId", "Description");

            return View();
        }


        public IActionResult Home()
        {
            return LoadViews();
        }

        public IActionResult Dashboard()
        {
            return LoadViews();
        }


    }
}