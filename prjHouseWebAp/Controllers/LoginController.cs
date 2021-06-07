using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using prjHouseWebAp.Models;
using System.Web.Security;

namespace prjHouseWebAp.Controllers
{
    public class LoginController : Controller
    {
        HouseDBEntities db = new HouseDBEntities();
        public ActionResult Index()
        {
            return View();
        }
        // GET: Login
        [HttpPost]
        public ActionResult Index(string 帳號, string 密碼)
        {

            var member = db.會員
                .Where(m => m.帳號 == 帳號 && m.密碼 == 密碼)
                .FirstOrDefault();
            if (member != null)
            {
                FormsAuthentication.RedirectFromLoginPage
                    (member.帳號, true);
                if (member.角色 == "管理者")
                {
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Member");
                }
            }
            ViewBag.IsLogin = true;
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(會員 member)
        {
            string userid = member.帳號;
            var tempMember = db.會員.Where(m => m.帳號 == userid).FirstOrDefault();
            if (tempMember != null)
            {
                ViewBag.IsMember = true;
                return View();
            }
            db.會員.Add(member);
            db.SaveChanges();
            return View("Msg");
        }


    }
}