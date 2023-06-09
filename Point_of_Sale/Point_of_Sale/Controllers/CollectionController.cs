using Microsoft.AspNetCore.Mvc;

namespace Point_of_Sale.Controllers
{
    public class CollectionController : Controller
    {
        public IActionResult LoadViews()
        {
            ViewBag.DateNow = DateTime.Now;
            ViewBag.Username = HttpContext.Session.GetString("FullName");
            ViewBag.UserId = HttpContext.Session.GetString("UserId"); 
            ViewBag.UserType = HttpContext.Session.GetString("UserType"); 

            return View();
        }

        public IActionResult Collection()
        {
            return LoadViews();
        }
    }
}
