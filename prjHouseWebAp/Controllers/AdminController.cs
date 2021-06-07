using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using prjHouseWebAp.Models;

namespace prjHouseWebAp.Controllers
{


    public class AdminController : Controller
    {

        HouseDBEntities db = new HouseDBEntities();

        // GET: Admin
        [Authorize]
        public ActionResult Index()
        {
            string uid = User.Identity.Name;
            string role = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().角色;
            if (role != "管理者")
            {
                return RedirectToAction("Index", "PermissionErrorMsg", new { msg = "您的身份無管理會員的權限" });
            }
            return View(db.會員.ToList());
        }

        [Authorize]
        public ActionResult Delete(string userid)
        {
            string uid = User.Identity.Name;
            string role = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().角色;
            if (role != "管理者")
            {
                return RedirectToAction("Index", "PermissionErrorMsg", new { msg = "您的身份無管理會員的權限" });
            }
            var member = db.會員.Where(m => m.帳號 == userid).FirstOrDefault();
            db.會員.Remove(member);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [Authorize]
        public ActionResult Edit(string userid)
        {
            string uid = User.Identity.Name;
            string role = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().角色;
            if (role != "管理者")
            {
                return RedirectToAction("Index", "PermissionErrorMsg", new { msg = "您的身份無管理會員的權限" });
            }

            var member = db.會員.Where(m => m.帳號 == userid).FirstOrDefault();
            return View(member);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(string 帳號, string 姓名, string 密碼, string 電話, string 角色)
        {
            string uid = User.Identity.Name;
            string role = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().角色;
            if (role != "管理者")
            {
                return RedirectToAction("Index", "PermissionErrorMsg", new { msg = "您的身份無管理會員的權限" });
            }

            var member = db.會員.Where(m => m.帳號 == 帳號).FirstOrDefault();
            member.姓名 = 姓名;
            member.密碼 = 密碼;
            member.角色 = 角色;
            member.電話 = 電話;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Details(string userid)
        {
            string uid = User.Identity.Name;
            string role = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().角色;
            if (role != "管理者")
            {
                return RedirectToAction("Index", "PermissionErrorMsg", new { msg = "您的身份無管理會員的權限" });
            }

            var house = db.地點名稱.Where(m=>m.帳號==userid)
                .OrderByDescending(m=>m.地點編號).ToList();
            return View(house);
        }

    }
}