using Microsoft.AspNetCore.Mvc;

namespace Point_of_Sale.Controllers
{
    public class CollectionController : Controller
    {
        public IActionResult LoadViews()
        {
            ViewBag.DateNow = DateTime.Now;
            ViewBag.Username = Request.Cookies["FullName"];
            ViewBag.UserId = Request.Cookies["UserId"];

            return View();
        }

        public IActionResult Collection()
        {
            return LoadViews();
        }
    }
}
