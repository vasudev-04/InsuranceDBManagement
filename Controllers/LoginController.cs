using InsuranceMgmtDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace InsuranceMgmtDB.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login

        InsuranceMgtDbEntities1 db = new InsuranceMgtDbEntities1();

        public ActionResult Index()
        {

            return View();
        }

        //public ActionResult Home()
        //{
        //    return View();
        //}


       [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(tbl_users obj)
        {

            if (!ModelState.IsValid)
            {
                
                return View(obj);
            }

            else
            {
                var user = from u in db.tbl_users where u.emailid == obj.emailid && u.password == obj.password select u;

                 if(user.Count() == 0)
                {
                    ModelState.AddModelError("", "Invalid Credentials");
                    return View(obj);
                }

                else
                {
                    var user1 = (from u in db.tbl_users where u.emailid == obj.emailid && u.password == obj.password select u).FirstOrDefault();
                    Session["UserKey"] = Guid.NewGuid();

                    Session["LastLogin"] = user1.last_login_date;
                    user1.last_login_date = DateTime.Now;
                    db.SaveChanges();
                    Session["UserId"] = user1.user_id;
                    Session["UserName"] = user1.user_name;
                    return RedirectToAction("Index","Insurance");
                }
            }

            

        }

    }
}