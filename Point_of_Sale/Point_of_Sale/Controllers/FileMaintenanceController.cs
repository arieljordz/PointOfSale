using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Versioning;
using Point_of_Sale.DTO;
using Point_of_Sale.Interface;
using Point_of_Sale.Models;
using Point_of_Sale.Models.DBContext;
using System.Data;

namespace Point_of_Sale.Controllers
{
    public class FileMaintenanceController : Controller
    {
        private readonly PointOfSaleDbContext db;
        private readonly IGlobal global;

        public FileMaintenanceController(PointOfSaleDbContext context, IGlobal global_rep)
        {
            db = context;
            global = global_rep;
        }

        public IActionResult LoadViews()
        {
            ViewBag.DateNow = DateTime.Now;
            ViewBag.Username = Request.Cookies["FullName"];
            ViewBag.UserId = Request.Cookies["UserId"];
            ViewBag.UserType = Request.Cookies["UserType"];
            var userType = db.tbl_userType.OrderBy(x => x.Id).ToList();
            ViewBag.cmbUserType = new SelectList(userType, "Id", "Description");

            return View();
        }


        public IActionResult ProductDetails()
        {
            return LoadViews();
        }
        public IActionResult Brand()
        {
            return LoadViews();
        }
        public IActionResult Supplier()
        {
            return LoadViews();
        }
        public IActionResult User()
        {
            return LoadViews();
        }

        public IActionResult UserType()
        {
            return LoadViews();
        }

        public IActionResult Bank()
        {
            return LoadViews();
        }


        public IActionResult LoadUsers()
        {
            var list = db.tbl_user.ToList();
            List<object> data = new List<object>();
            foreach (var item in list)
            {
                var type = db.tbl_userType.Where(x => x.Id == item.UsertypeId).Select(x => x.Description).FirstOrDefault();
                var obj = new
                {
                    Id = item.Id,
                    FullName = item.FullName,
                    UserType = type == null ? "" : type,
                    UserName = item.UserName,
                    Password = item.Password,
                    DateRegistered = global.FormatDateMMDDYYYY(item.DateRegistered.ToShortDateString()),
                };
                data.Add(obj);
            }
            return Json(new { data = data });
        }

        [HttpPost]
        public IActionResult SaveUser(tbl_User user)
        {
            try
            {
                if (user.Id != 0)
                {
                    var qry = db.tbl_user.Where(x => x.Id == user.Id).SingleOrDefault();
                    qry.FullName = user.FullName;
                    qry.MiddleName = user.MiddleName;
                    qry.LastName = user.LastName;
                    qry.FullName = user.FirstName + " " + user.MiddleName.ToCharArray()[0] + ". " + user.LastName;
                    qry.UsertypeId = user.UsertypeId;
                    qry.UserName = user.UserName;
                    qry.Password = user.Password;
                    db.SaveChanges();
                }
                else
                {
                    user.DateRegistered = DateTime.Now;
                    user.FullName = user.FirstName + " " + user.MiddleName.ToCharArray()[0] + ". " + user.LastName;
                    db.tbl_user.Add(user);
                    db.SaveChanges();

                }
                return Json(new { success = true });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }

        public IActionResult UpdateUser(int Id)
        {
            try
            {
                var data = db.tbl_user.Where(x => x.Id == Id).SingleOrDefault();
                return Json(new { data = data });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message });
            }
        }

        public IActionResult RemoveUser(int Id)
        {
            try
            {
                var data = db.tbl_user.Where(x => x.Id == Id).SingleOrDefault();
                db.tbl_user.Remove(data);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        public IActionResult LoadUserType()
        {
            var list = db.tbl_userType.ToList();
            List<object> data = new List<object>();
            foreach (var item in list)
            {
                var obj = new
                {
                    Id = item.Id,
                    Description = item.Description,
                };
                data.Add(obj);
            }
            return Json(new { data = data });
        }

        [HttpPost]
        public IActionResult SaveUserType(tbl_UserType user)
        {
            try
            {
                if (user.Id != 0)
                {
                    var qry = db.tbl_userType.Where(x => x.Id == user.Id).SingleOrDefault();
                    qry.Description = user.Description;
                    db.SaveChanges();
                }
                else
                {
                    db.tbl_userType.Add(user);
                    db.SaveChanges();

                }
                return Json(new { success = true });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }

        public IActionResult UpdateUserType(int Id)
        {
            try
            {
                var data = db.tbl_userType.Where(x => x.Id == Id).SingleOrDefault();
                return Json(new { data = data });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message });
            }
        }

        public IActionResult RemoveUserType(int Id)
        {
            try
            {
                var data = db.tbl_userType.Where(x => x.Id == Id).SingleOrDefault();
                db.tbl_userType.Remove(data);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        public IActionResult LoadBank()
        {
            var list = db.tbl_bank.ToList();
            List<object> data = new List<object>();
            foreach (var item in list)
            {
                var obj = new
                {
                    Id = item.Id,
                    Description = item.Description,
                };
                data.Add(obj);
            }
            return Json(new { data = data });
        }

        [HttpPost]
        public IActionResult SaveBank(tbl_Bank bank)
        {
            try
            {
                if (bank.Id != 0)
                {
                    var qry = db.tbl_bank.Where(x => x.Id == bank.Id).SingleOrDefault();
                    qry.Description = bank.Description;
                    db.SaveChanges();
                }
                else
                {
                    db.tbl_bank.Add(bank);
                    db.SaveChanges();

                }
                return Json(new { success = true });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }

        public IActionResult UpdateBank(int Id)
        {
            try
            {
                var data = db.tbl_bank.Where(x => x.Id == Id).SingleOrDefault();
                return Json(new { data = data });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message });
            }
        }

        public IActionResult RemoveBank(int Id)
        {
            try
            {
                var data = db.tbl_bank.Where(x => x.Id == Id).SingleOrDefault();
                db.tbl_bank.Remove(data);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        public IActionResult LoadBrand()
        {
            var list = db.tbl_brand.ToList();
            List<object> data = new List<object>();
            foreach (var item in list)
            {
                var obj = new
                {
                    Id = item.Id,
                    Description = item.Description,
                };
                data.Add(obj);
            }
            return Json(new { data = data });
        }

        [HttpPost]
        public IActionResult SaveBrand(tbl_Brand brand)
        {
            try
            {
                if (brand.Id != 0)
                {
                    var qry = db.tbl_brand.Where(x => x.Id == brand.Id).SingleOrDefault();
                    qry.Description = brand.Description;
                    db.SaveChanges();
                }
                else
                {
                    db.tbl_brand.Add(brand);
                    db.SaveChanges();

                }
                return Json(new { success = true });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }

        public IActionResult UpdateBrand(int Id)
        {
            try
            {
                var data = db.tbl_brand.Where(x => x.Id == Id).SingleOrDefault();
                return Json(new { data = data });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message });
            }
        }

        public IActionResult RemoveBrand(int Id)
        {
            try
            {
                var data = db.tbl_brand.Where(x => x.Id == Id).SingleOrDefault();
                db.tbl_brand.Remove(data);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        public IActionResult LoadSupplier()
        {
            var list = db.tbl_supplier.ToList();
            List<object> data = new List<object>();
            foreach (var item in list)
            {
                var obj = new
                {
                    Id = item.Id,
                    Description = item.Description,
                };
                data.Add(obj);
            }
            return Json(new { data = data });
        }

        [HttpPost]
        public IActionResult SaveSupplier(tbl_Supplier supplier)
        {
            try
            {
                if (supplier.Id != 0)
                {
                    var qry = db.tbl_supplier.Where(x => x.Id == supplier.Id).SingleOrDefault();
                    qry.Description = supplier.Description;
                    db.SaveChanges();
                }
                else
                {
                    db.tbl_supplier.Add(supplier);
                    db.SaveChanges();

                }
                return Json(new { success = true });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }

        public IActionResult UpdateSupplier(int Id)
        {
            try
            {
                var data = db.tbl_supplier.Where(x => x.Id == Id).SingleOrDefault();
                return Json(new { data = data });
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message });
            }
        }

        public IActionResult RemoveSupplier(int Id)
        {
            try
            {
                var data = db.tbl_supplier.Where(x => x.Id == Id).SingleOrDefault();
                db.tbl_supplier.Remove(data);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}
