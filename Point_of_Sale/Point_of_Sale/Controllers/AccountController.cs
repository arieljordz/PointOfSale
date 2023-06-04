using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Point_of_Sale.Interface;
using Point_of_Sale.Models.DBContext;

namespace Point_of_Sale.Controllers
{
    public class AccountController : Controller
    {
        private readonly PointOfSaleDbContext db;
        private readonly IGlobal global;

        public AccountController(PointOfSaleDbContext context, IGlobal global_rep)
        {
            db = context;
            global = global_rep;
        }

        public IActionResult Account()
        {
            ViewBag.DateNow = DateTime.Now;
            var userType = db.tbl_userType.OrderBy(x => x.Id).ToList();
            ViewBag.cmbUserType = new SelectList(userType, "Id", "Description");

            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password, int UsertypeId)
        {
            var qry = db.tbl_user.Where(x => x.UserName == username && x.Password == password && x.UsertypeId == UsertypeId).FirstOrDefault();

            if (qry != null)
            {
                //DateTime dateNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                // Create a new cookie
                var cookieOptions = new CookieOptions
                {
                    // Set additional options if needed
                    Expires = DateTime.Now.AddMinutes(30),
                    Secure = true,
                    HttpOnly = true
                };
                var UserType = db.tbl_userType.Where(x => x.Id == qry.UsertypeId).FirstOrDefault().Description;
                // Add the cookie to the response
                Response.Cookies.Append("FullName", qry.FullName, cookieOptions);
                Response.Cookies.Append("UserType", UserType, cookieOptions);
                Response.Cookies.Append("UserId", qry.Id.ToString(), cookieOptions);

                if (UsertypeId == 1)
                {
                    //return RedirectToAction("Dashboard", "Home");
                    //return RedirectToAction("ProductDetails", "Products");
                    //return RedirectToAction("User", "FileMaintenance");
                    //return RedirectToAction("PointOfSale", "PointOfSale");
                    //return RedirectToAction("Inventory", "Inventory");
                    return RedirectToAction("Collection", "Collection");
                }
                else
                {
                    return RedirectToAction("PointOfSale", "PointOfSale");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View("Home");
            }
        }

    }
}
