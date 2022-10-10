using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InsuranceMgmtDB.Models;
using System.Web.Security;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using InsuranceMgmtDB;
using PagedList;
using PagedList.Mvc;
using System.Text;
using OfficeOpenXml;
using System.IO;
using OfficeOpenXml.Style;
using System.Drawing;

namespace InsuranceMgmtDB.Controllers
{
    public class InsuranceController : Controller
    {
        // GET: Insurance

        InsuranceMgtDbEntities1 db = new InsuranceMgtDbEntities1();
        public ActionResult Index()
        {
            
            return View();
        }


        


                                        /// <summary>
                                        /// ALTERNATE METHODS TO SEND DATA FROM DB TO EXCEL SHEET USING MVC
                                        /// </summary>
                                        /// 


        public void ExcelExport()
        {
           List<tbl_users> TableData = db.tbl_users.ToList();
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet ws = package.Workbook.Worksheets.Add("TotalUsers");
                ws.Cells["A1"].LoadFromCollection(TableData, true);
                ws.Cells.AutoFitColumns();



                using (ExcelRange rng = ws.Cells["A1:O1"])
                {

                    rng.Style.Font.Bold = true;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid; //Set Pattern for the background to Solid 
                    rng.Style.Fill.BackgroundColor.SetColor(Color.Gold); //Set color to DarkGray 
                    rng.Style.Font.Color.SetColor(Color.Black);

                   

                    foreach(ExcelRangeBase i in ws.Cells[2, 1, ws.Dimension.End.Row, ws.Dimension.End.Column])
                    {
                        if (string.IsNullOrEmpty(i.Text)) continue;

                        var text = i.Text;

                        if (text.Equals("Active"))
                        {
                            //using (ExcelRange ing = ws.Cells[2, 2, 19, 9])
                            //{

                                Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#ffee75");
                                ws.Cells[i.Start.Row, i.Start.Column - 7, i.Start.Row, i.Start.Column].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ws.Cells[i.Start.Row, i.Start.Column - 7, i.Start.Row, i.Start.Column].Style.Fill.BackgroundColor.SetColor(colFromHex);


                                //ing.Style.Font.Bold = true;
                                //ing.Style.Fill.PatternType = ExcelFillStyle.Solid; //Set Pattern for the background to Solid 
                                //ing.Style.Fill.BackgroundColor.SetColor(Color.Black); //Set color to DarkGray 
                                //ing.Style.Font.Color.SetColor(Color.Yellow);
                            //}
                        }

                    }

                    


                }




                ////FileInfo fi = new FileInfo(@"E:\userslist.xlsx"); ---------------------> //NOT TO DOWNLOAD THE REMODIFIED EXCEL FILE AGAIN AND AGAIN
                //package.SaveAs(fi);
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=" + "Report.xlsx");
                Response.BinaryWrite(package.GetAsByteArray());
                Response.End();

            }




        }
                                    /// <summary>
                                    /// ALTERNATE METHODS TO SEND DATA FROM DB TO EXCEL SHEET USING MVC
                                    /// </summary>
        public void AdvisorExcel()
        {
            List<tbl_advisor> Advisor = db.tbl_advisor.ToList();
            using (ExcelPackage package = new ExcelPackage())
            {
               
                ExcelWorksheet ws = package.Workbook.Worksheets.Add("AdvisorList");
                ws.Cells["A1"].LoadFromCollection(Advisor,true);


                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=" + "Report.xlsx");
                Response.BinaryWrite(package.GetAsByteArray());
                Response.End();

            }
        }
                            /// <summary>
                            /// ALTERNATE METHODS TO SEND DATA FROM DB TO EXCEL SHEET USING MVC
                            /// </summary>

        public void ExportToExcel()
        {
            List<tbl_users> users = db.tbl_users.ToList();
            ExcelPackage package = new ExcelPackage();
            ExcelWorksheet es = package.Workbook.Worksheets.Add("Advisor List");
            es.Cells["A1"].Value = "User Id";
            es.Cells["B1"].Value = "User Name";
            es.Cells["C1"].Value = "Group Function ID";
            es.Cells["D1"].Value = "Password";
            es.Cells["E1"].Value = "Role Name";
            es.Cells["F1"].Value = "Advisor Id";
            es.Cells["G1"].Value = "Email iD";
            es.Cells["H1"].Value = "Contact no";
            es.Cells["I1"].Value = "Status";
            es.Cells["J1"].Value = "Created on";
            es.Cells["K1"].Value = "Created on";
            es.Cells["L1"].Value = "Modified on";
            es.Cells["M1"].Value = "Modified By";
            es.Cells["N1"].Value = "Display Name";
            es.Cells["O1"].Value = "Last Login Date";
            es.Cells["A2"].LoadFromCollection(users);
            for (int i = 0, r = 2; i < users.Count; i++)
            {
                es.Cells["A" + r.ToString()].Value = users[i].last_login_date.ToString();
                r++;

            }

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename=" + "Report.xlsx");
            Response.BinaryWrite(package.GetAsByteArray());
            Response.End();




        }

        [HttpPost]
        public JsonResult ValidationError(tbl_users obj)
        {
            return Json(!db.tbl_users.Any(s => s.user_name == obj.user_name || s.emailid == obj.emailid), JsonRequestBehavior.AllowGet);
        }



        public ActionResult GetUsers(int? page)
        {
            //return View(db.tbl_advisor.ToList());
            return View(db.tbl_users.ToList().ToPagedList(page ?? 1, 5));
        }

        public ActionResult NewUser()
        {
           
            return View();
        }

        [HttpPost]
        public ActionResult NewUser(tbl_users obj)
        {
            db.tbl_users.Add(obj);
            db.SaveChanges();
            return RedirectToAction("GetUsers");
        }


        public ActionResult Edit(int id)
        {
            tbl_users res = db.tbl_users.Where(s => s.user_id == id).FirstOrDefault();
            return View(res);
        }

        [HttpPost]

        public ActionResult Edit(int? id)
        {
          tbl_users cust  = db.tbl_users.Where(s => s.user_id == id).FirstOrDefault();

            //db.Entry(obj4).State = System.Data.Entity.EntityState.Modified;
            UpdateModel(cust);
           // db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            
            return RedirectToAction("GetUsers");
        }
        [HttpPost]
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }


        public ActionResult ResetPwd()
        {
            ResetPwdModel RPwd = new ResetPwdModel();

            return View();
        }
        //[HttpPost]


        [HttpPost]
        public ActionResult ResetPwd(ResetPwdModel chgPwd)
        {
            var chageUser = db.tbl_users.Find(Session["UserId"]);
            if (chageUser.password == chgPwd.currentPassword)
            {
                chageUser.password = chgPwd.confirmNewPassword;
                db.SaveChanges();
                ViewBag.message = "Password Updated Successfully";
                return View();
            }
            else
            {
                ViewBag.message = "Invalid Password";
                return View();
            }


        }
        //public ActionResult ResetPwd(ResetPwdModel obj3)
        //{
        //    var userId = Session["UserId"];
        //    var obj1 = db.tbl_users.Find(userId);
        //    if(obj1.password==obj3.currentPassword && obj3.NewPassword==obj3.confirmNewPassword)
        //    {
        //        obj1.password = obj3.NewPassword;
        //        db.SaveChanges();
        //        ViewBag.message = "Password Reset Successfully";
        //        return RedirectToAction("Index", "Insurance");

        //    }

        //    else
        //    {
        //        ViewBag.message = "Password Reset Failed";
        //        return View();
        //    }
        //    return View();
        //}


        [HttpGet]
        public ActionResult NewAdvisor()
        {
            ViewBag.message = db.tbl_advisor_experience_level.Where(s => s.status == "Active").ToList().Select(p => p.advisor_exp_level).ToList();
            return View();
        }


        [HttpPost]
       
        public ActionResult NewAdvisor(tbl_advisor obj, string Life, string CIS, string GI, string GEB)
        {
            var userNames = from i in  db.tbl_advisor where i.advisor_name == obj.advisor_name select i;
            if (userNames.Count() > 0)
            {
                ModelState.AddModelError("", "Advisor Name Already Exists!!!!!");
                return View();
            }

            else
            {

          
                    tbl_advisor_experience_level obh = db.tbl_advisor_experience_level.Where(s => s.advisor_exp_level == obj.tbl_advisor_experience_level.advisor_exp_level).FirstOrDefault();
                    obj.advisor_exp_level_id = obh.advisor_exp_level_id;
                    StringBuilder sb = new StringBuilder();
                    if (Convert.ToBoolean(Life))
                    {
                        sb.Append("Life");
                    }

                    if (Convert.ToBoolean(CIS))
                    {
                        sb.Append(",").Append("CIS");
                    }

                    if (Convert.ToBoolean(GI))
                    {
                        sb.Append(",").Append("GI");
                    }

                    if (Convert.ToBoolean(GEB))
                    {
                        sb.Append(",").Append("GEB");
                    }

                    obj.activity = sb.ToString();

                    db.tbl_advisor.Add(obj);

                    db.SaveChanges();
                ViewBag.add = "Success";
            //ViewBag.message = db.tbl_advisor_experience_level.Where(s => s.status == "Active").ToList().Select(p => p.advisor_exp_level).ToList();
            return RedirectToAction("NewAdvisor", "Insurance");
            }
        }



        public ActionResult AdvisorList(int? page)
         {
            //int pagesize = 5;
            //int pageindex = 1;
            //pageindex = page.HasValue ? Convert.ToInt32(page)
            //        : 1;
            //tbl_users users = new tbl_users();
            //IPagedList<tbl_users> tblusers = null;
            //List<tbl_users> listofusers = new List<tbl_users>();
            //listofusers = db.tbl_users.ToList();
            //var order = from user in listofusers
            //            orderby user.user_id
            //            select user;

            //tblusers = order.ToPagedList(pageindex, pagesize);
            //return View(tblusers);
            //ViewBag.add = null;
            return View(db.tbl_advisor.ToList().ToPagedList(page?? 1, 5));
        }

        public ActionResult Tab()
        {
            return View();
        }


    }
}
