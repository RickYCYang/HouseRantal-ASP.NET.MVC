using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using prjHouseWebAp.Models;
using prjHouseWebAp.ViewModels;

namespace prjHouseWebAp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        HouseDBEntities db = new HouseDBEntities();

        public ActionResult HouseList(string type)
        {
            var house = db.地點名稱.Where(m => m.類型 == type).OrderByDescending(m => m.地點編號).ToList();
            return View(house);
        }

        public ActionResult HouseDetails(int id)
        {
            var house = db.地點名稱.Where(m => m.地點編號 == id).FirstOrDefault();
            string userid = house.帳號;
            var member = db.會員.Where(m => m.帳號 == userid).FirstOrDefault();
            int houseid = house.地點編號;
            var message = db.評語.Where(m => m.地點編號 == houseid).OrderByDescending(m=>m.評語編號).ToList();
            MemberHouseViewModel vm = new MemberHouseViewModel();
            vm.House = house;
            vm.Member = member;
            vm.Message = message;
            return View(vm);
        }


        public ActionResult AddMsg(int id)
        {
            ViewBag.HouseId = id;
            return View();
        }
        [HttpPost]
        public ActionResult AddMsg(int 地點編號, string 圖示, string  說明)
        {
            評語 msg = new 評語();
            msg.地點編號 = 地點編號;
            msg.圖示 = 圖示;
            msg.說明 = 說明;
            msg.日期 = DateTime.Now.ToString();
            db.評語.Add(msg);
            db.SaveChanges();
            return RedirectToAction("HouseDetails", new { id=地點編號});
        }



    }
}